using System;
using AutoMapper;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Dal;
using TimeTracker.Data.Base;

using DalUser = TimeTracker.Dal.Entities.User;
using DtoUser = TimeTracker.Data.Entities.User;
using DalUserRole = TimeTracker.Dal.Entities.UserRole;
using DtoUserRole = TimeTracker.Data.Entities.UserRole;
using DalUserSession = TimeTracker.Dal.Entities.UserSession;
using DtoUserSession = TimeTracker.Data.Entities.UserSession;
using DalUserSetting = TimeTracker.Dal.Entities.UserSetting;
using DtoUserSetting = TimeTracker.Data.Entities.UserSetting;
using DalUserState = TimeTracker.Dal.Entities.UserState;
using DtoUserState = TimeTracker.Data.Entities.UserState;
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
                //todo: configure!
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
    }
}