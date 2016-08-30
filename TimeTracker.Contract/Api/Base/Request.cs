using System.Runtime.Serialization;

namespace TimeTracker.Contract.Api.Base
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
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