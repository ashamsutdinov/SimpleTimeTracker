using System;
using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    [DataContract]
    public class UserSession :
        IUserSession
    {
        [DataMember]
        public int Id { get; set; }

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