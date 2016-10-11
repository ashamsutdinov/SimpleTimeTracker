using System.Runtime.Serialization;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Contract.Data.Session
{
    [DataContract]
    public class SessionData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string TicketSalt { get; set; }

        [DataMember]
        public string TicketHash { get; set; }
    }
}