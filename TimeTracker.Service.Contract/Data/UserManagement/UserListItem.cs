using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.UserManagement
{
    [DataContract]
    public class UserListItem 
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string StateId { get; set; }

        [DataMember]
        public string StateDescription { get; set; }

        [DataMember]
        public List<UserRoleItem> Roles { get; set; }

        [DataMember]
        public List<UserSettingValueItem> Settings { get; set; }
    }
}