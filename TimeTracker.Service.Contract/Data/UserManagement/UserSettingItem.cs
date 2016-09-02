using System.Runtime.Serialization;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Contract.Data.UserManagement
{
    [DataContract]
    public class UserSettingItem :
        KeyValueItem
    {
        [DataMember]
        public string Value { get; set; }
    }
}