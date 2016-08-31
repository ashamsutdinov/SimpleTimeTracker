using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Contract.Log;
using TimeTracker.Service.Base.Utils;
using TimeTracker.Service.Contract;
using TimeTracker.Service.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data.Rest;

namespace TimeTracker.Service.Base
{
    public abstract class TimeTrackerServiceBase :
        ITimeTrackerService
    {
        private readonly CryptographyHelper _cryptographyHelper = new CryptographyHelper();

        protected readonly IUserDataProvider UserDataProvider;

        protected readonly ITimeRecordDataProvider TimeRecordDataProvider;

        protected readonly ILogger Log;

        protected TimeTrackerServiceBase()
        {
            var serviceResolver = ServiceResolverFactory.Get();
            Log = serviceResolver.Resolve<ILogger>();
            Log.Debug(this, "Creating TimeTrackerServiceBase...");
            UserDataProvider = serviceResolver.Resolve<IUserDataProvider>();
            TimeRecordDataProvider = serviceResolver.Resolve<ITimeRecordDataProvider>();
            Log.Debug(this, "TimeTrackerServiceBase created");
        }

        public Response<long> Heartbit(Request<long> request)
        {
            return SafeInvoke((user, session) => DateTime.UtcNow.Ticks, request, true);
        }

        public Response<SessionState[]> Handshake(Request request)
        {
            return SafeInvoke((user, session) =>
            {
                if (user == null)
                {
                    Log.Info(this, "Anonymous user connected: {0}", request.ClientId);
                    return new[] { SessionState.Anonymous };
                }
                var result = new List<SessionState> { SessionState.LoggedInUser };
                if (user.Roles.Any(r => r.Id == "administrator"))
                {
                    result.Add(SessionState.LoggedInAdministrator);
                }
                if (user.Roles.Any(r => r.Id == "manager"))
                {
                    result.Add(SessionState.LoggedInManager);
                }
                Log.Info(this, "User connected: {0}", result.Cast<object>().ToArray());
                return result.ToArray();
            }, request);
        }

        public virtual Response<string> GetNonce(Request request)
        {
            return SafeInvoke((user, session) => _cryptographyHelper.GetNonce(request.ClientId), request, true);
        }

        public virtual Response<TicketData> Login(Request<LoginData> request)
        {
            return SafeInvoke((userNull, sessionNull) =>
            {
                //check if user already logged in
                if (userNull != null)
                {
                    throw new AuthenticationException("User already logged in");
                }
                
                //check password syntax
                PasswordData passwordData;
                var validPasswordSyntax = _cryptographyHelper.VerifyPasswordSyntax(request.Data.Password, out passwordData);
                if (!validPasswordSyntax)
                {
                    throw new AuthenticationException("Invalid request");
                }

                //check if user exists
                var login = request.Data.Login;
                var user = UserDataProvider.GetUser(login);
                if (user == null)
                {
                    throw new AuthenticationException("Invalid login or password");
                }

                var hash = _cryptographyHelper.HashPassword(passwordData.Password, user.PasswordSalt);
                if (hash != user.PasswordHash)
                {
                    throw new AuthenticationException("Invalid login or password");
                }

                if (user.StateId != "active")
                {
                    throw new AuthenticationException("User was disabled or deleted");
                }

                var session = UserDataProvider.CreateNewSession(user.Id, request.ClientId);
                //create ticket
                var ticket = _cryptographyHelper.GetSessionTicket(session.Id, user.Id, passwordData.ClientId);
                session.Ticket = ticket;
                UserDataProvider.SaveSession(session);

                return new TicketData
                {
                    Ticket = ticket
                };
            }, request);
        }

        public Response<string> Logout(Request request)
        {
            return SafeInvoke((user, session) =>
            {
                session.Expired = true;
                UserDataProvider.SaveSession(session);
                return "Bye";
            }, request);
        }

        public Response<int> Register(Request<RegistrationData> request)
        {
            return SafeInvoke((userNull, session) =>
            {
                //check if user already logged in
                if (userNull != null)
                {
                    throw new AuthenticationException("User already logged in");
                }

                //check password syntax
                PasswordData passwordData;
                var validPasswordSyntax = _cryptographyHelper.VerifyPasswordSyntax(request.Data.Password, out passwordData);
                if (!validPasswordSyntax)
                {
                    throw new AuthenticationException("Invalid request");
                }

                //check if user exists
                var login = request.Data.Login;
                var existingUser = UserDataProvider.GetUser(login);
                if (existingUser != null)
                {
                    throw new AuthenticationException("Login cannot be used");
                }

                var salt = _cryptographyHelper.CreateSalt();
                var hash = _cryptographyHelper.HashPassword(passwordData.Password, salt);
                var user = UserDataProvider.RegisterUser(login, request.Data.Name, hash, salt);

                return user.Id;
            }, request);
        }

        public Response<int> CreateTimeRecord(Request<TimeRecordData> request)
        {
            return SafeInvoke((user, session) =>
            {
                var data = request.Data;
                if (data.Hours > 24)
                {
                    throw new InvalidOperationException("The number of working hours per day may not exceed 24 hours");
                }
                if (data.Hours <= 0)
                {
                    throw new InvalidOperationException("Duration required");
                }
                var existingDayRecord = TimeRecordDataProvider.GetUserDayRecordByDate(user.Id, data.Date);
                if (existingDayRecord != null && existingDayRecord.TotalHours + data.Hours > 24)
                {
                    throw new InvalidOperationException("The number of working hours per day may not exceed 24 hours");
                }
                var saved = TimeRecordDataProvider.SaveTimeRecord(0, user.Id, data.Date, data.Caption, data.Hours);
                return saved.Id;
            }, request);
        }

        public Response<TimeRecordItemList> LoadTimeRecords(Request<TimeRecordsFilterData> request)
        {
            return SafeInvoke((user, session) =>
            {
                var data = request.Data;
                int? userId = null;
                if (data.LoadAllUsers && user.Roles.All(r => r.Id != "administrator"))
                {
                    throw new UnauthorizedAccessException("Only administrator can request all user's time records");
                }
                if (!data.LoadAllUsers)
                {
                    userId = user.Id;
                }

                int total;
                var timeRecords = TimeRecordDataProvider.GetTimeRecords(userId, data.From, data.To, data.PageNumber, data.PageSize, out total);
                var result = new TimeRecordItemList
                {
                    Items = timeRecords.Cast<TimeRecordItem>().ToList(),
                    PageNumber = data.PageNumber,
                    PageSize = data.PageSize,
                    TotalItems = total,
                    TotalPages = total / data.PageSize + 1
                };
                return result;
            }, request);
        }


        private Response<TData> SafeInvoke<TData>(Func<IUser, IUserSession, TData> action, Request request, bool skipTicketValidation = false)
        {
            try
            {
                IUser user = null;
                IUserSession session = null;
                var ticketIsNotEmpty = !string.IsNullOrEmpty(request.Ticket);
                if (ticketIsNotEmpty && !skipTicketValidation)
                {
                    SessionData sessionData;

                    //verify ticket syntax
                    if (!_cryptographyHelper.VerifyTicketSyntax(request.Ticket, out sessionData))
                    {
                        Log.Warning(this, "Invalid ticket: {0}", request.Ticket);
                        return Response<TData>.NotAcceptable("Invalid ticket");
                    }

                    //check if user exists
                    user = UserDataProvider.GetUser(sessionData.UserId);
                    if (user == null)
                    {
                        Log.Warning(this, "User was not found: {0}", sessionData.UserId);
                        return Response<TData>.Unauthorized("User was not found");
                    }

                    //check if session exists
                    session = UserDataProvider.GetUserSession(sessionData.Id);
                    if (session == null)
                    {
                        Log.Warning(this, "Session was not found: {0}", sessionData.Id);
                        return Response<TData>.NotAcceptable("Invalid session");
                    }

                    //check if received ticket is equal with ticket stored in db
                    if (session.Ticket != request.Ticket)
                    {
                        Log.Warning(this, "Invalid ticket: {0}, expected: {1}", request.Ticket, session.Ticket);
                        return Response<TData>.NotAcceptable("Invalid ticket");
                    }

                    //check if session and owned by user
                    if (user.Id != session.UserId)
                    {
                        Log.Warning(this, "User have no access to requested session: {0}, sessionId: {1}", user.Id,
                            session.Id);
                        return Response<TData>.Forbidden("User have no access to requested session");
                    }

                    //check if session was expired
                    if (session.Expired || session.DateTime.AddSeconds(session.Expiration) < DateTime.UtcNow)
                    {
                        session.Expired = true;
                        UserDataProvider.SaveSession(session);
                        Log.Warning(this, "Session was expired: {0}", session.Id);
                        return Response<TData>.NotAcceptable("Session was expired");
                    }

                    //ticket/session is valid, authentication passed

                    //advance session
                    session.DateTime = DateTime.UtcNow;
                    UserDataProvider.SaveSession(session);
                }

                var data = action(user, session);
                return Response<TData>.Success(data);
            }
            catch (InvalidOperationException ioex)
            {
                Log.Error(this, ioex, "Invalid operation");
                return Response<TData>.NotAcceptable(ioex.Message);
            }
            catch (UnauthorizedAccessException uaex)
            {
                Log.Error(this, uaex, "Unauthorized exception");
                return Response<TData>.Forbidden(uaex.Message);
            }
            catch (AuthenticationException aex)
            {
                Log.Error(this, aex, "Authentication exception");
                return Response<TData>.Unauthorized(aex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(this, ex, "Unhandled exception");
                return Response<TData>.Fail(ex.Message);
            }
        }
    }
}