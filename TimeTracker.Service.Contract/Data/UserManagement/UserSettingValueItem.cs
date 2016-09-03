using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.UserManagement
{
    [DataContract]
    public class UserSettingValueItem :
        UserSettingItem
    {
        [DataMember]
        public string Value { get; set; }
    }
}