using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Contract.Data.UserManagement
{
    [DataContract]
    public class UserList :
        PagedItemList<UserListItem>
    {
        [DataMember]
        public List<UserRoleItem> AllRoles { get; set; }

        [DataMember]
        public List<UserSettingItem> AllSettings { get; set; }

        [DataMember]
        public List<UserStateItem> AllStates { get; set; } 

        public UserList(IEnumerable<IUser> source, int pageNumber, int pageSize, int totalItems, IUserDataProvider userDataProvider) : 
            base(pageNumber, pageSize, totalItems)
        {
            Items = new List<UserListItem>();
            foreach (var user in source)
            {
                Items.Add(ConvertFromDtoUser(user));
            }
            AllRoles = userDataProvider.GetAllUserRoles().Select(r => new UserRoleItem
            {
                Id = r.Id,
                Description = r.Description
            }).ToList();
            AllStates = userDataProvider.GetAllUserStates().Select(s => new UserStateItem
            {
                Id = s.Id,
                Description = s.Description
            }).ToList();
            AllSettings = userDataProvider.GetAllUserSettings().Select(s => new UserSettingItem
            {
                Id = s.Id,
                Description = s.Description
            }).ToList();
        }

        public static UserListItem ConvertFromDtoUser(IUser user)
        {
            var userItem = new UserListItem
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.Name,
                Roles = user.Roles.Select(r => new UserRoleItem
                {
                    Id = r.Id,
                    Description = r.Description
                }).ToList(),
                Settings = user.Settings.Select(s => new UserSettingValueItem
                {
                    Id = s.Id,
                    Value = s.Value,
                    Description = s.Description
                }).ToList(),
                StateId = user.StateId,
                StateDescription = user.State.Description
            };
            return userItem;
        }
    }
}