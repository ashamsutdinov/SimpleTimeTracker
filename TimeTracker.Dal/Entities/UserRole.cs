using System.Data.SqlClient;

namespace TimeTracker.Dal.Entities
{
    public class UserRole
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public static UserRole Read(SqlDataReader reader)
        {
            return new UserRole
            {
                Id = (string) reader["Id"],
                Description = (string) reader["Description"]
            };
        }
    }
}