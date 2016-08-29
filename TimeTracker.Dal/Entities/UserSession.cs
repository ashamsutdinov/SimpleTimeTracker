using System;
using System.Data;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class UserSession :
        Entity<int>
    {
        public int UserId { get; set; }

        public string Ticket { get; set; }

        public bool Expired { get; set; }

        public DateTime DateTime { get; set; }

        public int Expiration { get; set; }

        public UserSession()
        {

        }

        public UserSession(IDataRecord reader) :
            base(reader)
        {
            UserId = (int)reader["UserId"];
            Ticket = (string)reader["Ticket"];
            Expired = (bool)reader["Expired"];
            DateTime = (DateTime)reader["DateTime"];
            Expiration = (int)reader["Expiration"];
        }
    }
}