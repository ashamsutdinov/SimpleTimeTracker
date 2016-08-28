using System.Data.SqlClient;

namespace TimeTracker.Dal.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string Name { get; set; }

        public string StateId { get; set; }

        public static User Read(SqlDataReader reader)
        {
            return new User
            {
                Id = (int) reader["Id"],
                Login = (string) reader["Login"],
                PasswordHash = (string) reader["PasswordHash"],
                PasswordSalt = (string) reader["PasswordSalt"],
                Name = (string) reader["Name"],
                StateId = (string) reader["StateId"]
            };
        }
    }
}