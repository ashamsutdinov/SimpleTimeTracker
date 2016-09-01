using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data
{
    [DataContract]
    public class UserSettingItemList :
        ItemList<UserSettingItem>
    {
        [DataMember]
        public int UserId { get; set; }
    }
}