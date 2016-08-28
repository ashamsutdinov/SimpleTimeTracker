using System;
using System.Data.SqlClient;

namespace TimeTracker.Dal.Entities
{
    public class UserSession
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Ticket { get; set; }

        public bool Expired { get; set; }

        public DateTime DateTime { get; set; }

        public int Expiration { get; set; }

        public UserSession Read(SqlDataReader reader)
        {
            return new UserSession
            {
                Id = (int) reader["Id"],
                UserId = (int) reader["UserId"],
                Ticket = (string) reader["Ticket"],
                Expired = (bool) reader["Expired"],
                DateTime = (DateTime) reader["DateTime"],
                Expiration = (int) reader["Expiration"]
            };
        }
    }
}