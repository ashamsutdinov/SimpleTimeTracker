using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.ServiceModel.Web;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Contract.Log;
using TimeTracker.Service.Base.Utils;
using TimeTracker.Service.Base.Validation.Authentication;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Base.Validation.Registration;
using TimeTracker.Service.Base.Validation.Session;
using TimeTracker.Service.Base.Validation.TimeRecords;
using TimeTracker.Service.Base.Validation.UserManagement;
using TimeTracker.Service.Contract;
using TimeTracker.Service.Contract.Data.Authentication;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.Registration;
using TimeTracker.Service.Contract.Data.Session;
using TimeTracker.Service.Contract.Data.TimeRecords;
using TimeTracker.Service.Contract.Data.UserManagement;

namespace TimeTracker.Service.Base
{
    public abstract class TimeTrackerServiceBase :
        ITimeTrackerService
    {
        private readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly HttpStatusCode[] _failedResponseStatusCodes = new HttpStatusCode[]
        {
            HttpStatusCode.Unauthorized, 
            HttpStatusCode.Forbidden, 
            HttpStatusCode.NotAcceptable, 
            HttpStatusCode.BadRequest,
            HttpStatusCode.InternalServerError
        };

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

        private readonly ValidationRule[] _userSettingsValidationRules;

        private readonly ValidationRule[] _userManagementValidationRules;

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
                new UserIsNotLoggedInValidationRule(),
                new UserByLoginExistsValidationRule(UserDataProvider),
                new MatchPasswordValidationRule(_cryptographyHelper, UserDataProvider),
                new UserIsActiveValudationRule(UserDataProvider)
            };

            _registrationValidationRules = new ValidationRule[]
            {
                new UserIsNotLoggedInValidationRule(),
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

            _userSettingsValidationRules = new ValidationRule[]
            {
                new UserSettingsOwnerExistsValidationRule(UserDataProvider), 
                new UserSettingsEditRightsValidationRule("manager") 
            };

            _userManagementValidationRules = new ValidationRule[]
            {
                new UserManagementPermissionsValidationRule("manager"),
                new UserManagementUserExistsValidationRule(UserDataProvider)
            };
        }


        public Response<HeartbitData> Heartbit(string tick)
        {
            //it seems to be unrealistic to catch any exception here
            var nTick = long.Parse(tick);
            var request = new Request<long> { Data = nTick };
            return SafeInvoke(request, (user, session) => new HeartbitData
            {
                ClientTime = nTick,
                ServerTime = (long)DateTime.UtcNow.Subtract(_epoch).TotalMilliseconds
            }, true);
        }

        public Response<SessionState[]> Handshake()
        {
            var request = new Request();
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

        public virtual Response<TicketData> Login(LoginData data)
        {
            var request = new Request<LoginData> { Data = data };
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

        public Response<string> Logout()
        {
            var request = new Request();
            return SafeInvoke(request, (user, session) =>
            {
                session.Expired = true;
                UserDataProvider.SaveSession(session);
                return "Bye";
            });
        }

        public Response<int> Register(RegistrationData data)
        {
            var request = new Request<RegistrationData> { Data = data };
            return SafeInvoke(request, (userNull, session) =>
            {
                var login = request.Data.Login;
                var salt = _cryptographyHelper.CreateSalt();
                var hash = _cryptographyHelper.HashPassword(request.Data.Password, salt);
                var user = UserDataProvider.RegisterUser(login, request.Data.Name, hash, salt);
                return user.Id;
            }, _registrationValidationRules);
        }

        public Response<int> SaveTimeRecord(string id, TimeRecordData data)
        {
            var request = new Request<TimeRecordData> { Data = data };
            return SafeInvoke(request, (user, session) =>
            {
                var saved = TimeRecordDataProvider.SaveTimeRecord(data.Id, user.Id, data.Date, data.Caption, data.Hours);
                return saved.Id;
            }, _saveTimeRecordValidationRules);
        }

        public Response<int> DeleteTimeRecord(Request<TimeRecordData> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                TimeRecordDataProvider.DeleteTimeRecord(request.Data.Id);
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
                var timeRecords = TimeRecordDataProvider.GetTimeRecords(data.LoadAllUsers ? (int?)null : user.Id, data.From, data.To, data.PageNumber, data.PageSize, out total);
                var result = new TimeRecordItemList(timeRecords.ToList(), data.PageNumber, data.PageSize, total, UserDataProvider, TimeRecordDataProvider);
                return result;
            }, _getUserTimeRecordsValidationRules);
        }

        public Response<UserSettingItemList> GetUserSettings(Request<UserSettingItemList> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var requestedUserId = request.Data.UserId > 0 ? request.Data.UserId : user.Id;
                var allSettings = UserDataProvider.GetAllUserSettings();
                var userSettings = UserDataProvider.GetUserSettings(requestedUserId);
                return new UserSettingItemList
                {
                    UserId = requestedUserId,
                    Items = allSettings.Select(s => new UserSettingValueItem
                    {
                        Id = s.Id,
                        Description = s.Description,
                        Value = userSettings.Where(us => us.Id == s.Id).Select(us => us.Value).FirstOrDefault()
                    }).ToList()
                };
            }, _userSettingsValidationRules);
        }

        public Response<int> SaveUserSettings(Request<UserSettingItemList> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var requestedUserId = request.Data.UserId > 0 ? request.Data.UserId : user.Id;
                var requestedUser = UserDataProvider.GetUser(requestedUserId);
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
            }, _userSettingsValidationRules);
        }

        public Response<UserList> GetUsers(Request<UserListData> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                int total;
                var users = UserDataProvider.GetUsers(request.Data.PageNumber, request.Data.PageSize, out total);
                var result = new UserList(users, request.Data.PageNumber, request.Data.PageSize, total, UserDataProvider);
                return result;
            }, _userManagementValidationRules);
        }

        public Response<UserListItem> GetUser(Request<UserListItem> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var requestedUser = UserDataProvider.GetUser(request.Data.Id);
                var convertedUser = UserList.ConvertFromDtoUser(requestedUser);

                //this is to ensure ALL possible settings will be delivered to the client
                var allSettings = UserDataProvider.GetAllUserSettings();
                var userSettings = UserDataProvider.GetUserSettings(request.Data.Id);

                convertedUser.Settings = allSettings.Select(s => new UserSettingValueItem
                {
                    Id = s.Id,
                    Description = s.Description,
                    Value = userSettings.Where(us => us.Id == s.Id).Select(us => us.Value).FirstOrDefault()
                }).ToList();

                return convertedUser;

            }, _userManagementValidationRules);
        }

        public Response<int> SaveUser(Request<UserListItem> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var userToSave = UserDataProvider.GetUser(request.Data.Id);
                userToSave.Name = request.Data.Name;
                userToSave.StateId = request.Data.StateId;
                userToSave.Roles.Clear();
                foreach (var role in request.Data.Roles)
                {
                    userToSave.Roles.Add(UserDataProvider.PrepareRole(role.Id));
                }
                userToSave.Settings.Clear();
                foreach (var setting in request.Data.Settings)
                {
                    userToSave.Settings.Add(UserDataProvider.PrepareUserSetting(setting.Id, setting.Value));
                }
                UserDataProvider.SaveUser(userToSave);
                return userToSave.Id;
            }, _userManagementValidationRules);
        }

        public Response<int> DeleteUser(Request<UserListItem> request)
        {
            return SafeInvoke(request, (user, session) => UserDataProvider.DeleteUser(request.Data.Id), _userManagementValidationRules);
        }

        public Response<int> ResetPassword(Request<UserListItem> request)
        {
            return SafeInvoke(request, (user, session) =>
            {
                var requestedUser = UserDataProvider.GetUser(request.Data.Id);
                var newSalt = _cryptographyHelper.CreateSalt();
                var newPassword = "123456";
                var newPasswordHash = _cryptographyHelper.HashPassword(newPassword, newSalt);
                requestedUser.PasswordHash = newPasswordHash;
                requestedUser.PasswordSalt = newSalt;
                UserDataProvider.SaveUser(requestedUser);
                return requestedUser.Id;
            }, _userManagementValidationRules);
        }

        private Response<TData> SafeInvoke<TData>(Request request, Func<IUser, IUserSession, TData> action, ValidationRule[] requestValidationRules = null)
        {
            if (WebOperationContext.Current != null)
            {
                var wcfRequest = WebOperationContext.Current.IncomingRequest;
                var headers = wcfRequest.Headers;
                request.Ticket = headers["Ticket"];
                request.ClientId = headers["ClientId"];
            }
            var response = SafeInvoke(request, action, false, requestValidationRules);
            if (_failedResponseStatusCodes.Contains(response.StatusCode))
            {
                throw new WebFaultException<Response<TData>>(response, response.StatusCode);
            }
            return response;
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