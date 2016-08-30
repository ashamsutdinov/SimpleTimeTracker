using System.Runtime.Serialization;

namespace TimeTracker.Contract.Api
{
    [DataContract]
    public class TicketData
    {
        public string Ticket { get; set; }
    }
}