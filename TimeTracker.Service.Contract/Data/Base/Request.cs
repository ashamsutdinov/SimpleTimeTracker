using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Base
{
    [DataContract]
    public class Request
    {
        public string ClientId { get; set; }

        public string Ticket { get; set; }
    }

    [DataContract]
    public class Request<TData> :
        Request
    {
        [DataMember]
        public TData Data { get; set; }
    }
}