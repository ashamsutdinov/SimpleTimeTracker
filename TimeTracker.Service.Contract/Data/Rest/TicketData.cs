using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Rest
{
    [DataContract]
    public class TicketData
    {
        [DataMember]
        public string Ticket { get; set; }
    }
}