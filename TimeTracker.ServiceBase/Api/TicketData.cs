using System.Runtime.Serialization;

namespace TimeTracker.ServiceBase.Api
{
    [DataContract]
    public class TicketData
    {
        [DataMember]
        public string Ticket { get; set; }
    }
}