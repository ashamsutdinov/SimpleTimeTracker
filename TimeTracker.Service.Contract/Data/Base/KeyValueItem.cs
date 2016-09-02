using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Base
{
    [DataContract]
    public class KeyValueItem
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}