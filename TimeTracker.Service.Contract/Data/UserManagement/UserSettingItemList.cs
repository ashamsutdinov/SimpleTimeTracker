using System.Runtime.Serialization;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Contract.Data.UserManagement
{
    [DataContract]
    public class UserSettingItemList :
        ItemList<UserSettingValueItem>
    {
        [DataMember]
        public int UserId { get; set; }
    }
}