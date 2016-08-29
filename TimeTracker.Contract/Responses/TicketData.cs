using System.Runtime.Serialization;

namespace TimeTracker.Contract.Responses
{
    [DataContract]
    public class TicketData
    {
        public string Ticket { get; set; }
    }
}