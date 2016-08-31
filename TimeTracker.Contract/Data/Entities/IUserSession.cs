using System;

namespace TimeTracker.Contract.Data.Entities
{
    public interface IUserSession :
        IEntity<int>
    {
        int UserId { get; set; }

        string ClientId { get; set; }

        string Ticket { get; set; }

        bool Expired { get; set; }

        DateTime DateTime { get; set; }

        int Expiration { get; set; }
    }
}