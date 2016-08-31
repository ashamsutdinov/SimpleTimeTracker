using System;
using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data.Entities
{
    [DataContract]
    public class UserSession :
        Entity<int>,
        IUserSession
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string Ticket { get; set; }

        [DataMember]
        public bool Expired { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public int Expiration { get; set; }
    }
}