using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Session
{
    [DataContract]
    public class HeartbitData
    {
        [DataMember]
        public long ClientTime { get; set; }

        [DataMember]
        public long ServerTime { get; set; }
    }
}