using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using TimeTracker.Contract.Api;
using TimeTracker.Contract.Api.Base;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Contract.IoC;
using TimeTracker.Contract.Log;
using TimeTracker.Contract.Utils;

namespace TimeTracker.Contract
{
    public abstract class TimeTrackerServiceBase :
        ITimeTrackerService
    {
        private readonly CryptographyHelper _cryptographyHelper = new CryptographyHelper();

        protected readonly IUserDataProvider UserDataProvider;

        protected readonly ITimeRecordDataProvider TimeTrackingDataProvider;

        protected readonly ILogger Log;

        protected TimeTrackerServiceBase()
        {
            var serviceResolver = ServiceResolverFactory.Get();
            Log = serviceResolver.Resolve<ILogger>();
            Log.Debug(this, "Creating TimeTrackerServiceBase...");
            UserDataProvider = serviceResolver.Resolve<IUserDataProvider>();
            TimeTrackingDataProvider = serviceResolver.Resolve<ITimeRecordDataProvider>();
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

                    //check if user exists
                    user = UserDataProvider.GetUser(sessionData.UserId);
                    if (user == null)
                    {
                        Log.Warning(this, "User was not found: {0}", sessionData.UserId);
                        return Response<TData>.Unauthorized("User was not found");
                    }

                    //check if session and owned by user
                    if (user.Id != session.UserId)
                    {
                        Log.Warning(this, "User have no access to requested session: {0}, sessionId: {1}", user.Id, session.Id);
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
            catch (UnauthorizedAccessException uaex)
            {
                return TraceUnauthorizedException<TData>(uaex);
            }
            catch (AuthenticationException aex)
            {
                return TraceAuthenticationException<TData>(aex);
            }
            catch (Exception ex)
            {
                return TraceException<TData>(ex);
            }
        }

        private Response<TData> TraceException<TData>(Exception ex)
        {
            Log.Error(this, ex, "Unhandled exception");
            return Response<TData>.Fail(ex.Message);
        }

        private Response<TData> TraceUnauthorizedException<TData>(UnauthorizedAccessException ex)
        {
            Log.Error(this, ex, "Unauthorized exception");
            return Response<TData>.Forbidden(ex.Message);
        }

        private Response<TData> TraceAuthenticationException<TData>(AuthenticationException ex)
        {
            Log.Error(this, ex, "Authentication exception");
            return Response<TData>.Unauthorized(ex.Message);
        }
    }
}