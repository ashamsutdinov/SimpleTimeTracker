using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TimeTracker.Dal.Entities;
using TimeTracker.Dal.Utils;

namespace TimeTracker.Dal
{
    // continue from GetUserSession
    public class UserDa :
        BaseDa
    {
        public int AbandonSession(int id)
        {
            var idParameter = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            return ExecuteNonQuery("[dbo].[AbandonSession]", idParameter);
        }

        public List<UserSession> GetActiveSessions(int userId)
        {
            var userIdParameter = new SqlParameter("@UserId", SqlDbType.Int) { Value = userId };
            var reader = ExecuteReader("[dbo].[GetActiveSessions]", userIdParameter);
            var result = new List<UserSession>();
            while (reader.Read())
            {
                result.Add(new UserSession(reader));
            }
            return result;
        }

        public UserSession SaveUserSession(UserSession session)
        {
            var idParameter = new SqlParameter("@Id", SqlDbType.Int) { Value = session.Id };
            var userIdParameter = new SqlParameter("@UserId", SqlDbType.Int) { Value = session.UserId };
            var clientIdParameter = new SqlParameter("@ClientId", SqlDbType.NVarChar) { Value = session.ClientId };
            var ticketParameter = new SqlParameter("@Ticket", SqlDbType.NVarChar) { Value = session.Ticket };
            var dateTimeParameter = new SqlParameter("@DateTime", SqlDbType.DateTime2) { Value = session.DateTime };
            var expirationParameter = new SqlParameter("@Expiration", SqlDbType.Int) { Value = session.Expiration };
            var reader = ExecuteReader("[dbo].[SaveUserSession]", idParameter, userIdParameter, clientIdParameter, ticketParameter, dateTimeParameter, expirationParameter);
            return reader.Read() ? new UserSession(reader) : null;
        }

        public UserSession GetUserSession(int id)
        {
            var idParameter = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            var reader = ExecuteReader("[dbo].[GetUserSession]", idParameter);
            return reader.Read() ? new UserSession(reader) : null;
        }

        public List<UserRole> GetAllUserRoles()
        {
            var reader = ExecuteReader("[dbo].[GetAllUserRoles]");
            var result = new List<UserRole>();
            while (reader.Read())
            {
                result.Add(new UserRole(reader));
            }
            return result;
        }

        public List<UserRole> GetUserRoles(int userId)
        {
            var userIdParameter = new SqlParameter("@UserId", SqlDbType.Int) { Value = userId };
            var reader = ExecuteReader("[dbo].[GetUserRoles]", userIdParameter);
            var result = new List<UserRole>();
            while (reader.Read())
            {
                result.Add(new UserRole(reader));
            }
            return result;
        }

        public List<UserSetting> GetAllUserSettings()
        {
            var reader = ExecuteReader("[dbo].[GetAllUserSettings]");
            var result = new List<UserSetting>();
            while (reader.Read())
            {
                result.Add(new UserSetting(reader));
            }
            return result;
        }

        public List<UserState> GetAllUserStates()
        {
            var reader = ExecuteReader("[dbo].[GetAllUserStates]");
            var result = new List<UserState>();
            while (reader.Read())
            {
                result.Add(new UserState(reader));
            }
            return result;
        }

        public User GetUser(int id)
        {
            var idParameter = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            var reader = ExecuteReader("[dbo].[GetUser]", idParameter);
            return reader.Read() ? new User(reader, true) : null;
        }

        public User GetUserByLogin(string login)
        {
            var idParameter = new SqlParameter("@Login", SqlDbType.NVarChar) { Value = login };
            var reader = ExecuteReader("[dbo].[GetUserByLogin]", idParameter);
            return reader.Read() ? new User(reader, true) : null;
        }

        public List<User> GetUsers(int pageNumber, int pageSize, out int total)
        {
            var pageNumberParameter = new SqlParameter("@PageNumber", SqlDbType.Int) {Value = pageNumber};
            var pageSizeParameter = new SqlParameter("@PageSize", SqlDbType.Int) {Value = pageSize};
            IDbCommand outCommand;
            var reader = ExecuteReader("[dbo].[GetUsers]", out outCommand, pageNumberParameter, pageSizeParameter);
            var result = new List<User>();
            while (reader.Read())
            {
                result.Add(new User(reader));
            }
            var outParameter = outCommand.Parameters["@Total"] as SqlParameter;
            total = outParameter != null ? (int) outParameter.Value : 0;
            return result;
        }
    }
}