using System.Runtime.Serialization;

namespace TimeTracker.Contract.Api
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