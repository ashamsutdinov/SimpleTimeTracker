using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Contract.Log;
using TimeTracker.Service.Base.Utils;
using TimeTracker.Service.Base.Validation.Authentication;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Base.Validation.Registration;
using TimeTracker.Service.Base.Validation.Session;
using TimeTracker.Service.Base.Validation.TimeRecord;
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

        private readonly ValidationRule[] _ticketValudationRules;

        private readonly ValidationRule[] _loginValidationRules;

        private readonly ValidationRule[] _registrationValidationRules;

        private readonly ValidationRule[] _saveTimeRecordValidationRules;

        private readonly ValidationRule[] _deleteTimeRecordValidationRules;

        private readonly ValidationRule[] _saveTimeRecordNoteValidationRules;

        private readonly ValidationRule[] _getUserTimeRecordsValidationRules;

        protected TimeTrackerServiceBase()
        {
            var serviceResolver = ServiceResolverFactory.Get();
            Log = serviceResolver.Resolve<ILogger>();
            UserDataProvider = serviceResolver.Resolve<IUserDataProvider>();
            TimeRecordDataProvider = serviceResolver.Resolve<ITimeRecordDataProvider>();
            Log.Debug(this, "TimeTrackerServiceBase created");

            _ticketValudationRules = new ValidationRule[]
            {
                new RequestTicketSyntaxValidationRule(_cryptographyHelper),
                new UserExistsValidationRule(),
                new SessionExistsValidationRule(), 
                new TicketConsistencyValidationRule(),
                new SessionOwnershipValidationRule(),
                new SessionIsNotExpiredValidationRule(UserDataProvider), 
            };

            _loginValidationRules = new ValidationRule[]
            {
                new UserAlreadyLoggedInPolicy(),
                new LoginPasswordValidationRule(_cryptographyHelper),
                new UserByLoginExistsPolicy(UserDataProvider),
                new MatchPasswordValidationRule(_cryptographyHelper, UserDataProvider),
                new UserIsActiveValudationRule(UserDataProvider)
            };

            _registrationValidationRules = new ValidationRule[]
            {
                new UserAlreadyLoggedInPolicy(),
                new RegistrationPasswordSyntaxValidationRule(_cryptographyHelper),
                new LoginCanBeUserValidationRule(UserDataProvider)
            };

            _saveTimeRecordValidationRules = new ValidationRule[]
            {
                new TimeRecordNonZeroDurationValidationRule(),
                new TimeRecordDurationValidationRule(),
                new TimeRecordExistsValidationRule(TimeRecordDataProvider),
                new TimeRecordEditRightsValidationRule(TimeRecordDataProvider, "administrator"),
                new TimeRecordTotalDuractionValidationRule(TimeRecordDataProvider)
            };

            _deleteTimeRecordValidationRules = new ValidationRule[]
            {
                new TimeRecordExistsValidationRule(TimeRecordDataProvider),
                new TimeRecordEditRightsValidationRule(TimeRecordDataProvider, "administrator")
            };

            _saveTimeRecordNoteValidationRules = new ValidationRule[]
            {
                new TimeRecordNoteExistsValidationRule(TimeRecordDataProvider),
                new TimeRecordNoteEditRightsValidationRule(TimeRecordDataProvider, "administrator")
            };

            _getUserTimeRecordsValidationRules = new ValidationRule[]
            {
                new TimeRecordListAccessRightsValidationRule("administrator") 
            };
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
                //at this point, login and password already verified by validation rules
                var user = UserDataProvider.GetUser(request.Data.Login);
                var session = UserDataProvider.CreateNewSession(user.Id, request.ClientId);
                var ticket = _cryptographyHelper.GetSessionTicket(session.Id, user.Id, request.ClientId);
                session.Ticket = ticket;
                UserDataProvider.SaveSession(session);
                return new TicketData
                {
                    Ticket = ticket
                };
            }, _loginValidationRules);
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
            }, _registrationValidationRules);
        }

        public Response<int> SaveTimeRecord(Request<TimeRecordData> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var data = request.Data;
                var saved = TimeRecordDataProvider.SaveTimeRecord(data.Id, user.Id, data.Date, data.Caption, data.Hours);
                return saved.Id;
            }, _saveTimeRecordValidationRules);
        }

        public Response<int> DeleteTimeRecord(Request<TimeRecordData> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                TimeRecordDataProvider.DeleteTimeRecordNote(request.Data.Id);
                return request.Data.Id;
            }, _deleteTimeRecordValidationRules);
        }

        public Response<int> SaveTimeRecordNote(Request<TimeRecordNoteData> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var note = TimeRecordDataProvider.PrepareTimeRecordNote(request.Data.DayRecordId, user.Id, request.Data.Text);
                TimeRecordDataProvider.SaveTimeRecordNote(note);
                return note.Id;
            }, _saveTimeRecordNoteValidationRules);
        }

        public Response<int> DeleteTimeRecordNote(Request<TimeRecordNoteData> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                TimeRecordDataProvider.DeleteTimeRecordNote(request.Data.Id);
                return request.Data.Id;
            }, _saveTimeRecordNoteValidationRules);
        }

        public Response<TimeRecordItemList> GetTimeRecords(Request<TimeRecordsFilterData> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var data = request.Data;
                int total;
                var timeRecords = TimeRecordDataProvider.GetTimeRecords(data.LoadAllUsers ? (int?) null : user.Id, data.From, data.To, data.PageNumber, data.PageSize, out total);
                var result = new TimeRecordItemList(timeRecords.ToList(), data.PageNumber, data.PageSize, total, UserDataProvider, TimeRecordDataProvider);
                return result;
            }, _getUserTimeRecordsValidationRules);
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

        private Response<TData> SafeInvoke<TData>(Request request, Func<IUser, IUserSession, TData> action, ValidationRule[] requestValidationRules = null)
        {
            return SafeInvoke(request, action, false, requestValidationRules);
        }


        private Response<TData> SafeInvoke<TData>(Request request, Func<IUser, IUserSession, TData> action, bool skipTicketValidation, ValidationRule[] requestValidationRules = null)
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

                    foreach (var ticketValidationRule in _ticketValudationRules)
                    {
                        ticketValidationRule.Evaluate(user, session, request);
                    }

                    //advance session
                    session.DateTime = DateTime.UtcNow;
                    UserDataProvider.SaveSession(session);
                }

                if (requestValidationRules != null)
                {
                    foreach (var requestValidationRule in requestValidationRules)
                    {
                        requestValidationRule.Evaluate(user, session, request);
                    }
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