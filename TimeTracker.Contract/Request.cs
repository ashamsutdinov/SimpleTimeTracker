using System.Runtime.Serialization;

namespace TimeTracker.Contract
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public string ClientId { get; set; }
    }

    [DataContract]
    public class Request<TData> :
        Request
    {
        [DataMember]
        public TData Data { get; set; }
    }
}