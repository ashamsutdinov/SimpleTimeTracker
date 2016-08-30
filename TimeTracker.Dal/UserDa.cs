using System.Collections.Generic;
using System.Data;
using System.Linq;
using TimeTracker.Dal.Entities;
using TimeTracker.Dal.Utils;

namespace TimeTracker.Dal
{
    public class UserDa :
        BaseDa
    {
        public int AbandonSession(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            return ExecuteNonQuery("[dbo].[AbandonSession]", idParameter);
        }

        public List<UserSession> GetActiveSessions(int userId)
        {
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, userId);
            var reader = ExecuteReader("[dbo].[GetActiveSessions]", userIdParameter);
            return Read(reader, r => new UserSession(r));
        }

        public UserSession SaveUserSession(UserSession session)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, session.Id);
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, session.UserId);
            var clientIdParameter = CreateParameter("@ClientId", SqlDbType.NVarChar, session.ClientId);
            var ticketParameter = CreateParameter("@Ticket", SqlDbType.NVarChar, session.Ticket);
            var dateTimeParameter = CreateParameter("@DateTime", SqlDbType.DateTime2, session.DateTime);
            var expirationParameter = CreateParameter("@Expiration", SqlDbType.Int, session.Expiration);
            var reader = ExecuteReader("[dbo].[SaveUserSession]", idParameter, userIdParameter, clientIdParameter, ticketParameter, dateTimeParameter, expirationParameter);
            return ReadSingle(reader, r => new UserSession(r));
        }

        public UserSession GetUserSession(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            var reader = ExecuteReader("[dbo].[GetUserSession]", idParameter);
            return ReadSingle(reader, r => new UserSession(r));
        }

        public List<UserRole> GetAllUserRoles()
        {
            var reader = ExecuteReader("[dbo].[GetAllUserRoles]");
            return Read(reader, r => new UserRole(r));
        }

        public List<UserRole> GetUserRoles(int userId)
        {
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, userId);
            var reader = ExecuteReader("[dbo].[GetUserRoles]", userIdParameter);
            return Read(reader, r => new UserRole(r));
        }

        public List<UserSetting> GetAllUserSettings()
        {
            var reader = ExecuteReader("[dbo].[GetAllUserSettings]");
            return Read(reader, r => new UserSetting(r));
        }

        public List<UserToSetting> GetUserSessings(int userId)
        {
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, userId);
            var reader = ExecuteReader("[dbo].[GetUserSettings]", userIdParameter);
            return Read(reader, r => new UserToSetting(r));
        }

        public string GetUserSettingValue(int userId, string settingId)
        {
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, userId);
            var settingIdParameter = CreateParameter("@SettingId", SqlDbType.NVarChar, settingId);
            return ExecuteScalar<string>("[dbo].[GetUserSettingValue]", userIdParameter, settingIdParameter);
        }

        public List<UserState> GetAllUserStates()
        {
            var reader = ExecuteReader("[dbo].[GetAllUserStates]");
            return Read(reader, r => new UserState(r));
        }

        public User GetUser(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            var reader = ExecuteReader("[dbo].[GetUser]", idParameter);
            return ReadSingle(reader, r => new User(r, true));
        }

        public User GetUserByLogin(string login)
        {
            var idParameter = CreateParameter("@Login", SqlDbType.NVarChar, login);
            var reader = ExecuteReader("[dbo].[GetUserByLogin]", idParameter);
            return ReadSingle(reader, r => new User(r, true));
        }

        public List<User> GetUsers(int pageNumber, int pageSize, out int total)
        {
            var pageNumberParameter = CreateParameter("@PageNumber", SqlDbType.Int, pageNumber);
            var pageSizeParameter = CreateParameter("@PageSize", SqlDbType.Int, pageSize);
            IDbCommand outCommand;
            var reader = ExecuteReader("[dbo].[GetUsers]", out outCommand, pageNumberParameter, pageSizeParameter);
            var result = Read(reader, r => new User(r));
            total = GetOutputValue<int>(outCommand, "@Total");
            return result;
        }

        public User SaveUser(User user)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, user.Id);
            var loginParameter = CreateParameter("@Login", SqlDbType.NVarChar, user.Login);
            var passwordHashParameter = CreateParameter("@PasswordHash", SqlDbType.NVarChar, user.PasswordHash);
            var passwordSaltParameter = CreateParameter("@PasswordSalt", SqlDbType.NVarChar, user.PasswordSalt);
            var nameParameter = CreateParameter("@Name", SqlDbType.NVarChar, user.Name);
            var stateIdParameter = CreateParameter("@StateId", SqlDbType.NVarChar, user.StateId);
            var rolesParameter = CreateKeyCollectionParameter("@Roles", user.Roles.Select(r => r.Id));
            var settingsParameter = CreateKeyValueCollectionParameter("@Settings", user.Settings.Select(s => new KeyValuePair<string, string>(s.Id, s.Value)));
            var reader = ExecuteReader("[dbo].[SaveUser]", idParameter, loginParameter, passwordHashParameter, passwordSaltParameter, nameParameter, stateIdParameter, rolesParameter, settingsParameter);
            return ReadSingle(reader, r => new User(r, true));
        }
    }
}