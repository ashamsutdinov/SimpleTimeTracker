using System;
using System.Data;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class UserSession :
        Entity<int>
    {
        public int UserId { get; set; }

        public string ClientId { get; set; }

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
            UserId = Read<int>(reader, "UserId");
            ClientId = Read<string>(reader, "ClientId");
            Ticket = Read<string>(reader, "Ticket");
            Expired = Read<bool>(reader, "Expired");
            DateTime = Read<DateTime>(reader, "DateTime");
            Expiration = Read<int>(reader, "Expiration");
        }
    }
}