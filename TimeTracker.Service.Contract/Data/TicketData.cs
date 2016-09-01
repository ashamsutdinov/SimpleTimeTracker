using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data
{
    [DataContract]
    public class TicketData
    {
        [DataMember]
        public string Ticket { get; set; }
    }
}