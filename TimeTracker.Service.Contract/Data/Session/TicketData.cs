using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Session
{
    [DataContract]
    public class TicketData
    {
        [DataMember]
        public string Ticket { get; set; }
    }
}