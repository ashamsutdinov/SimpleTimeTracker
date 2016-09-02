using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Base
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