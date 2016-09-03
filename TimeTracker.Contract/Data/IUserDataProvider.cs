using System.Collections.Generic;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Contract.Data
{
    public interface IUserDataProvider
    {
        IUserSession GetUserSession(int id);

        IUser GetUser(int id);

        IUserRole PrepareRole(string id);

        IUserToSetting PrepareUserSetting(string id, string value);

        IUser GetUser(string login);

        IUserSession CreateNewSession(int userId, string clientId);

        IUserSession SaveSession(IUserSession session);

        IList<IUser> GetUsers(int pageNumber, int pageSize, out int total);

        int DeleteUser(int id);

        IUser RegisterUser(string login, string name, string passwordHash, string passwordSalt);

        string GetUserSettingValue(int userId, string key);

        IList<IUserSetting> GetAllUserSettings();

        IList<IUserToSetting> GetUserSettings(int userId);

        IList<IUserRole> GetAllUserRoles();

        IList<IUserState> GetAllUserStates();

        IUserToSetting PrepareUserSetting();

        IUser SaveUser(IUser user);
    }
}