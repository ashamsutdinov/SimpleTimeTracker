using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data
{
    [DataContract]
    public class UserSettingItem :
        KeyValueItem
    {
        [DataMember]
        public string Value { get; set; }
    }
}