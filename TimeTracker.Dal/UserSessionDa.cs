using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TimeTracker.Dal.Entities;
using TimeTracker.Dal.Utils;

namespace TimeTracker.Dal
{
    public class UserSessionDa :
        BaseDa
    {
        public int AbandonSession(int id)
        {
            var idParameter = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            return ExecuteNonQuery("[dbo].[AbandonSession]", idParameter);
        }

        public List<UserSession> GetActiveSession(int userId)
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
            var dateTimeParameter = new SqlParameter("@DateTime", SqlDbType.DateTime2) { Value = session.DateTime };
            var expirationParameter = new SqlParameter("@Expiration", SqlDbType.Int) { Value = session.Expiration };
            var reader = ExecuteReader("[dbo].[SaveUserSession]", idParameter, userIdParameter, dateTimeParameter, expirationParameter);
            return reader.Read() ? new UserSession(reader) : null;
        }
    }
}