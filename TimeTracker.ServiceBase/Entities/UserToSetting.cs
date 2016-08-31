using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.ServiceBase.Entities
{
    [DataContract]
    public class UserToSetting :
        UserSetting,
        IUserToSetting
    {
        [DataMember]
        public string Value { get; set; }
    }
}