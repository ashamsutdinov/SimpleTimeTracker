using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Contract.Log;
using TimeTracker.Service.Base.Security;
using TimeTracker.Service.Base.Utils;
using TimeTracker.Service.Contract;
using TimeTracker.Service.Contract.Data;

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
            UserDataProvider = serviceResolver.Resolve<IUserDataProvider>();
            TimeRecordDataProvider = serviceResolver.Resolve<ITimeRecordDataProvider>();
            Log.Debug(this, "TimeTrackerServiceBase created");
        }

        public Response<long> Heartbit(Request<long> request)
        {
            return SafeInvoke(request, (user, session) => DateTime.UtcNow.Ticks, true);
        }

        public Response<SessionState[]> Handshake(Request request)
        {
            return SafeInvoke(request, (user, session) =>
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
                Log.Info(this, "User connected: {0}", request.ClientId);
                return result.ToArray();
            });
        }

        public virtual Response<string> GetNonce(Request request)
        {
            return SafeInvoke(request, (user, session) => _cryptographyHelper.GetNonce(request.ClientId), true);
        }

        public virtual Response<TicketData> Login(Request<LoginData> request)
        {
            return SafeInvoke(request, (userNull, sessionNull) =>
            {
                //at this point, login and password already passed validation by security policies
                var user = UserDataProvider.GetUser(request.Data.Login);
                var session = UserDataProvider.CreateNewSession(user.Id, request.ClientId);
                var ticket = _cryptographyHelper.GetSessionTicket(session.Id, user.Id, request.ClientId);
                session.Ticket = ticket;
                UserDataProvider.SaveSession(session);
                return new TicketData
                {
                    Ticket = ticket
                };
            },
            new UserAlreadyLoggedInPolicy(),
            new PasswordSyntaxOnLoginPolicy(_cryptographyHelper),
            new UserByLoginExistsPolicy(UserDataProvider),
            new PasswordHashMatchPolicy(_cryptographyHelper, UserDataProvider),
            new UserActiveStatePolicy(UserDataProvider));
        }

        public Response<string> Logout(Request request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                session.Expired = true;
                UserDataProvider.SaveSession(session);
                return "Bye";
            });
        }

        public Response<int> Register(Request<RegistrationData> request)
        {
            return SafeInvoke(request, (userNull, session) =>
            {
                var passwordData = _cryptographyHelper.DecodeXorPassword(request.Data.Password);
                var login = request.Data.Login;
                var salt = _cryptographyHelper.CreateSalt();
                var hash = _cryptographyHelper.HashPassword(passwordData.Password, salt);
                var user = UserDataProvider.RegisterUser(login, request.Data.Name, hash, salt);
                return user.Id;
            },
            new UserAlreadyLoggedInPolicy(),
            new PasswordSyntaxOnRegistrationPolicy(_cryptographyHelper),
            new UserByLoginDoesExistsPolicy(UserDataProvider));
        }

        public Response<int> SaveTimeRecord(Request<TimeRecordData> request)
        {
            return SafeInvoke(request, (user, session) =>
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
                var ownerId = user.Id;
                var existingTimeRecordDuration = 0;
                var existingDayRecordId = 0;
                if (request.Data.Id > 0)
                {
                    var existingTimeRecord = TimeRecordDataProvider.GetTimeRecord(request.Data.Id);
                    if (existingTimeRecord != null)
                    {
                        var linkedDayRecord = TimeRecordDataProvider.GetDayRecord(existingTimeRecord.DayRecordId);
                        ownerId = linkedDayRecord.UserId;
                        existingTimeRecordDuration = existingTimeRecord.Hours;
                        existingDayRecordId = existingTimeRecord.DayRecordId;
                    }
                    else
                    {
                        throw new InvalidOperationException("Time record was not found");
                    }
                }
                if (ownerId != user.Id && user.Roles.All(r => r.Id != "administrator"))
                {
                    throw new UnauthorizedAccessException("Only administrator can edit time records");
                }
                var existingDayRecord = TimeRecordDataProvider.GetUserDayRecordByDate(ownerId, data.Date);

                if (existingDayRecord != null)
                {
                    int testTotalHours;
                    if (existingDayRecord.Id == existingDayRecordId)
                    {
                        //we edit the same day record
                        testTotalHours = existingDayRecord.TotalHours - existingTimeRecordDuration + request.Data.Hours;
                    }
                    else
                    {
                        //we edit another day record
                        testTotalHours = existingDayRecord.TotalHours + request.Data.Hours;
                    }
                    if (testTotalHours > 24)
                    {
                        throw new InvalidOperationException("The number of working hours per day may not exceed 24 hours");
                    }
                }
                var saved = TimeRecordDataProvider.SaveTimeRecord(data.Id, user.Id, data.Date, data.Caption, data.Hours);
                return saved.Id;
            });
        }

        public Response<int> DeleteTimeRecord(Request<int> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var existingTimeRecord = TimeRecordDataProvider.GetTimeRecord(request.Data);
                if (existingTimeRecord == null)
                {
                    throw new InvalidOperationException("Time record was not found");
                }
                var dayRecord = TimeRecordDataProvider.GetDayRecord(existingTimeRecord.DayRecordId);
                if (user.Id != dayRecord.UserId && user.Roles.All(r => r.Id != "administrator"))
                {
                    throw new UnauthorizedAccessException("Only administrator can delete user's time records");
                }
                TimeRecordDataProvider.DeleteTimeRecordNote(request.Data);
                return request.Data;
            });
        }

        public Response<int> SaveTimeRecordNote(Request<TimeRecordNoteData> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var dayRecord = TimeRecordDataProvider.GetDayRecord(request.Data.DayRecordId);
                if (dayRecord.UserId != user.Id && user.Roles.All(r => r.Id != "administrator"))
                {
                    throw new UnauthorizedAccessException("Only administrator can leave notes to time records");
                }
                ITimeRecordNote note;
                if (request.Data.Id > 0)
                {
                    note = TimeRecordDataProvider.GetTimeRecordNote(request.Data.Id);
                    if (note == null)
                    {
                        throw new InvalidOperationException("The record note was not found");
                    }
                    note.Text = request.Data.Text;
                }
                else
                {
                    note = TimeRecordDataProvider.PrepareTimeRecordNote(request.Data.DayRecordId, user.Id, request.Data.Text);
                }
                TimeRecordDataProvider.SaveTimeRecordNote(note);
                return note.Id;
            });
        }

        public Response<int> DeleteTimeRecordNote(Request<int> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var note = TimeRecordDataProvider.GetTimeRecordNote(request.Data);
                if (note == null)
                {
                    throw new InvalidOperationException("Time record note was not found");
                }
                if (note.UserId != user.Id && user.Roles.All(r => r.Id != "administrator"))
                {
                    throw new InvalidOperationException("Only administrator can delete user's time record notes");
                }
                TimeRecordDataProvider.DeleteTimeRecordNote(request.Data);
                return request.Data;
            });
        }

        public Response<TimeRecordItemList> GetTimeRecords(Request<TimeRecordsFilterData> request)
        {
            return SafeInvoke(request, (user, session) =>
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
                var result = new TimeRecordItemList(timeRecords.ToList(), data.PageNumber, data.PageSize, total, UserDataProvider, TimeRecordDataProvider);
                return result;
            });
        }

        public Response<UserSettingItemList> GetUserSettings(Request<int> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var requestedUserId = request.Data;
                if (requestedUserId == 0)
                {
                    requestedUserId = user.Id;
                }
                if (requestedUserId != user.Id && user.Roles.All(r => r.Id != "manager"))
                {
                    throw new UnauthorizedAccessException("Only manager can request settings for specific user");
                }

                var allSettings = UserDataProvider.GetAllUserSettings();
                var userSettings = UserDataProvider.GetUserSettings(requestedUserId);

                return new UserSettingItemList
                {
                    UserId = requestedUserId,
                    Items = allSettings.Select(s => new UserSettingItem
                    {
                        Id = s.Id,
                        Description = s.Description,
                        Value = userSettings.Where(us => us.Id == s.Id).Select(us => us.Value).FirstOrDefault()
                    }).ToList()
                };
            });
        }

        public Response<int> SaveUserSettings(Request<UserSettingItemList> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var requestedUserId = request.Data.UserId;
                if (requestedUserId == 0)
                {
                    requestedUserId = user.Id;
                }
                if (requestedUserId != user.Id && user.Roles.All(r => r.Id != "manager"))
                {
                    throw new UnauthorizedAccessException("Only manager can update settings for specific user");
                }
                var requestedUser = UserDataProvider.GetUser(requestedUserId);
                if (requestedUser == null)
                {
                    throw new InvalidOperationException("User was not found");
                }

                foreach (var itemEnu in request.Data.Items)
                {
                    var item = itemEnu;
                    var setting = requestedUser.Settings.FirstOrDefault(s => s.Id == item.Id);
                    if (setting == null)
                    {
                        setting = UserDataProvider.PrepareUserSetting();
                        setting.Id = item.Id;
                        setting.Description = item.Description;
                        requestedUser.Settings.Add(setting);
                    }
                    setting.Value = item.Value;
                }
                var updatedUser = UserDataProvider.SaveUser(requestedUser);
                return updatedUser.Id;
            });
        }

        private Response<TData> SafeInvoke<TData>(Request request, Func<IUser, IUserSession, TData> action, params SecurityPolicy[] policies)
        {
            return SafeInvoke(request, action, false, policies);
        }


        private Response<TData> SafeInvoke<TData>(Request request, Func<IUser, IUserSession, TData> action, bool skipTicketValidation, params SecurityPolicy[] requestPolicies)
        {
            try
            {
                IUser user = null;
                IUserSession session = null;
                var ticketIsNotEmpty = !string.IsNullOrEmpty(request.Ticket);
                if (ticketIsNotEmpty && !skipTicketValidation)
                {
                    var sessionData = _cryptographyHelper.DecodeXorTicket(request.Ticket);
                    user = UserDataProvider.GetUser(sessionData.UserId);
                    session = UserDataProvider.GetUserSession(sessionData.Id);

                    var ticketPolicies = new SecurityPolicy[]
                    {
                        new RequestTicketSyntaxPolicy(_cryptographyHelper),
                        new UserExistsPolicy(),
                        new SessionExistsPolicy(), 
                        new SessionTicketConsistencyPolicy(request.Ticket),
                        new SessionOwnershipPolicy(),
                        new SessionExpicationPolicy(UserDataProvider), 
                    };

                    foreach (var ticketPolicy in ticketPolicies)
                    {
                        ticketPolicy.Evaluate(user, session, request);
                    }

                    //advance session
                    session.DateTime = DateTime.UtcNow;
                    UserDataProvider.SaveSession(session);
                }

                foreach (var requestPolicy in requestPolicies)
                {
                    requestPolicy.Evaluate(user, session, request);
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