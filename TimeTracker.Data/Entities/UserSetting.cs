using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    [DataContract]
    public class UserSetting :
        IUserSetting
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}