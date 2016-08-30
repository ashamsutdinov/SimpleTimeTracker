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
            var result = ExecuteReader("[dbo].[GetActiveSessions]",
                reader => Read(reader, r => new UserSession(r)),
                userIdParameter);
            return result;
        }

        public UserSession SaveUserSession(UserSession session)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, session.Id);
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, session.UserId);
            var clientIdParameter = CreateParameter("@ClientId", SqlDbType.NVarChar, session.ClientId);
            var ticketParameter = CreateParameter("@Ticket", SqlDbType.NVarChar, session.Ticket);
            var dateTimeParameter = CreateParameter("@DateTime", SqlDbType.DateTime2, session.DateTime);
            var expirationParameter = CreateParameter("@Expiration", SqlDbType.Int, session.Expiration);
            var result = ExecuteReader("[dbo].[SaveUserSession]",
                reader => ReadSingle(reader, r => new UserSession(r)),
                idParameter,
                userIdParameter,
                clientIdParameter,
                ticketParameter,
                dateTimeParameter,
                expirationParameter);
            return result;
        }

        public UserSession GetUserSession(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            var result = ExecuteReader("[dbo].[GetUserSession]",
                reader => ReadSingle(reader, r => new UserSession(r)),
                idParameter);
            return result;
        }

        public List<UserRole> GetAllUserRoles()
        {
            var result = ExecuteReader("[dbo].[GetAllUserRoles]",
                reader => Read(reader, r => new UserRole(r)));
            return result;
        }

        public List<UserRole> GetUserRoles(int userId)
        {
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, userId);
            var result = ExecuteReader("[dbo].[GetUserRoles]",
                reader => Read(reader, r => new UserRole(r)),
                userIdParameter);
            return result;
        }

        public List<UserSetting> GetAllUserSettings()
        {
            var result = ExecuteReader("[dbo].[GetAllUserSettings]",
                reader => Read(reader, r => new UserSetting(r)));
            return result;
        }

        public List<UserToSetting> GetUserSessings(int userId)
        {
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, userId);
            var result = ExecuteReader("[dbo].[GetUserSettings]",
                reader => Read(reader, r => new UserToSetting(r)),
                userIdParameter);
            return result;
        }

        public string GetUserSettingValue(int userId, string settingId)
        {
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, userId);
            var settingIdParameter = CreateParameter("@SettingId", SqlDbType.NVarChar, settingId);
            return ExecuteScalar<string>("[dbo].[GetUserSettingValue]", userIdParameter, settingIdParameter);
        }

        public List<UserState> GetAllUserStates()
        {
            var result = ExecuteReader("[dbo].[GetAllUserStates]",
                reader => Read(reader, r => new UserState(r)));
            return result;
        }

        public User GetUser(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            var result = ExecuteReader("[dbo].[GetUser]",
                reader => ReadSingle(reader, r => new User(r, true)),
                idParameter);
            return result;
        }

        public User GetUserByLogin(string login)
        {
            var idParameter = CreateParameter("@Login", SqlDbType.NVarChar, login);
            var result = ExecuteReader("[dbo].[GetUserByLogin]",
                reader => ReadSingle(reader, r => new User(r, true)),
                idParameter);
            return result;
        }

        public List<User> GetUsers(int pageNumber, int pageSize, out int total)
        {
            var pageNumberParameter = CreateParameter("@PageNumber", SqlDbType.Int, pageNumber);
            var pageSizeParameter = CreateParameter("@PageSize", SqlDbType.Int, pageSize);
            IDbCommand outCommand;
            var result = ExecuteReader("[dbo].[GetUsers]",
                reader => Read(reader, r => new User(r)),
                out outCommand,
                pageNumberParameter,
                pageSizeParameter);
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
            var result = ExecuteReader("[dbo].[SaveUser]",
                reader => ReadSingle(reader, r => new User(r, true)),
                idParameter,
                loginParameter,
                passwordHashParameter,
                passwordSaltParameter,
                nameParameter,
                stateIdParameter,
                rolesParameter,
                settingsParameter);
            return result;
        }
    }
}