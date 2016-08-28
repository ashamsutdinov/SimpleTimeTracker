using System.Data.SqlClient;

namespace TimeTracker.Dal.Entities
{
    public class UserSetting
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public static UserSetting Read(SqlDataReader reader)
        {
            return new UserSetting
            {
                Id = (string) reader["Id"],
                Description = (string) reader["Description"]
            };
        }
    }
}