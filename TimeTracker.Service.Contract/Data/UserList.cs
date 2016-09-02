using System.Collections.Generic;
using System.Runtime.Serialization;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data
{
    [DataContract]
    public class UserList :
        PagedItemList<UserList>
    {
        [DataMember]
        public List<RoleItem> AllRoles { get; set; }

        [DataMember]
        public List<UserSettingItemList> AllSettings { get; set; }

        [DataMember]
        public List<UserStateItem> AllStates { get; set; } 

        public UserList(IEnumerable<IUser> source, int pageNumber, int pageSize, int totalItems, IUserDataProvider userDataProvider) : 
            base(pageNumber, pageSize, totalItems)
        {
            Items = new List<UserList>();
            foreach (var user in source)
            {
         //todo: complete
            }
        }
    }
}