using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Dal;
using TimeTracker.Data.Base;
using TimeTracker.Data.Entities;
using DalUser = TimeTracker.Dal.Entities.User;
using DalUserRole = TimeTracker.Dal.Entities.UserRole;
using DalUserSession = TimeTracker.Dal.Entities.UserSession;
using DtoUserSession = TimeTracker.Data.Entities.UserSession;
using DtoUserRole = TimeTracker.Data.Entities.UserRole;
using DalUserSetting = TimeTracker.Dal.Entities.UserSetting;
using DalUserState = TimeTracker.Dal.Entities.UserState;
using DalUserToSetting = TimeTracker.Dal.Entities.UserToSetting;
using DtoUserToSetting = TimeTracker.Data.Entities.UserToSetting;

namespace TimeTracker.Data
{
    public class UserDataProvider :
        DataProviderBase,
        IUserDataProvider
    {
        private readonly UserDa _userDa = new UserDa();

        public IUserSession GetUserSession(int id)
        {
            var dalSession = _userDa.GetUserSession(id);
            return Mapper.Map<DalUserSession, IUserSession>(dalSession);
        }

        public IUser GetUser(int id)
        {
            var dalUser = _userDa.GetUser(id);
            return Mapper.Map<DalUser, IUser>(dalUser);
        }

        public IUserRole PrepareRole(string id)
        {
            return new DtoUserRole { Id = id };
        }

        public IUserToSetting PrepareUserSetting(string id, string value)
        {
            return new DtoUserToSetting { Id = id, Value = value };
        }

        public IUser GetUser(string login)
        {
            var dalUser = _userDa.GetUserByLogin(login);
            return Mapper.Map<DalUser, IUser>(dalUser);
        }

        public IUserSession CreateNewSession(int userId, string clientId)
        {
            var session = new DtoUserSession
            {
                ClientId = clientId,
                DateTime = DateTime.UtcNow,
                Expiration = 1800,
                Expired = false,
                Ticket = null,
                UserId = userId
            };
            var dalSession = Mapper.Map<IUserSession, DalUserSession>(session);
            dalSession = _userDa.SaveUserSession(dalSession);
            return Mapper.Map<DalUserSession, IUserSession>(dalSession);
        }

        public IUserSession SaveSession(IUserSession session)
        {
            var dalSession = Mapper.Map<IUserSession, DalUserSession>(session);
            dalSession = _userDa.SaveUserSession(dalSession);
            return Mapper.Map<DalUserSession, IUserSession>(dalSession);
        }

        public IList<IUser> GetUsers(int pageNumber, int pageSize, out int total)
        {
            var dalUsers = _userDa.GetUsers(pageNumber, pageSize, out total);
            return dalUsers.Select(Mapper.Map<DalUser, IUser>).ToList();
        }

        public int DeleteUser(int id)
        {
            var user = GetUser(id);
            user.StateId = "deleted";
            SaveUser(user);
            return user.Id;
        }

        public IUser RegisterUser(string login, string name, string passwordHash, string passwordSalt)
        {
            var dalUser = new DalUser
            {
                Login = login,
                Name = name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                StateId = "active",
                Roles = new List<DalUserRole> { new DalUserRole { Id = "user" } },
                Settings = new List<DalUserToSetting>()
            };
            dalUser = _userDa.SaveUser(dalUser);
            return Mapper.Map<DalUser, IUser>(dalUser);
        }

        public string GetUserSettingValue(int userId, string key)
        {
            return _userDa.GetUserSettingValue(userId, key);
        }

        public IList<IUserSetting> GetAllUserSettings()
        {
            var dalSettings = _userDa.GetAllUserSettings();
            return dalSettings.Select(Mapper.Map<DalUserSetting, IUserSetting>).ToList();
        }

        public IList<IUserToSetting> GetUserSettings(int userId)
        {
            var dalSettings = _userDa.GetUserSessings(userId);
            return dalSettings.Select(Mapper.Map<DalUserToSetting, IUserToSetting>).ToList();
        }

        public IList<IUserRole> GetAllUserRoles()
        {
            var dalRoles = _userDa.GetAllUserRoles();
            return dalRoles.Select(Mapper.Map<DalUserRole, IUserRole>).ToList();
        }

        public IList<IUserState> GetAllUserStates()
        {
            var dalStates = _userDa.GetAllUserStates();
            return dalStates.Select(Mapper.Map<DalUserState, IUserState>).ToList();
        }

        public IUserToSetting PrepareUserSetting()
        {
            return new DtoUserToSetting();
        }

        public IUser SaveUser(IUser user)
        {
            var dalUser = Mapper.Map<IUser, DalUser>(user);
            dalUser = _userDa.SaveUser(dalUser);
            return Mapper.Map<DalUser, IUser>(dalUser);
        }
    }
}