using System.Runtime.Serialization;

namespace TimeTracker.Contract.Api
{
    [DataContract]
    public class TicketData
    {
        [DataMember]
        public string Ticket { get; set; }
    }
}