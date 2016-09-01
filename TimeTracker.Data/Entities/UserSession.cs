using System;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    public class UserSession :
        Entity<int>,
        IUserSession
    {
        public int UserId { get; set; }

        public string ClientId { get; set; }

        public string Ticket { get; set; }

        public bool Expired { get; set; }

        public DateTime DateTime { get; set; }

        public int Expiration { get; set; }
    }
}