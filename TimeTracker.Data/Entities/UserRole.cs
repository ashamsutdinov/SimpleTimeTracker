using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    [DataContract]
    public class UserRole :
        IUserRole
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}