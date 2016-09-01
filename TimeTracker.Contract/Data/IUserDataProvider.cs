using System.Collections;
using System.Collections.Generic;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Contract.Data
{
    public interface IUserDataProvider
    {
        IUserSession GetUserSession(int id);

        IUser GetUser(int id);

        IUser GetUser(string login);

        IUserSession CreateNewSession(int userId, string clientId);

        IUserSession SaveSession(IUserSession session);

        IUser RegisterUser(string login, string name, string passwordHash, string passwordSalt);

        string GetUserSettingValue(int userId, string key);

        IList<IUserSetting> GetAllUserSettings();

        IList<IUserToSetting> GetUserSettings(int userId);

        IUserToSetting PrepareUserSetting();

        IUser SaveUser(IUser user);
    }
}