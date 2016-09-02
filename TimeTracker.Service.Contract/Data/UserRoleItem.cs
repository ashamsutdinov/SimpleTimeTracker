using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data
{
    [DataContract]
    public class UserRoleItem
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}