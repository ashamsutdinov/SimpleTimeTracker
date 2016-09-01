using System.Collections.Generic;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    public class User :
        Entity<int>,
        IUser
    {
        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public string Name { get; set; }

        public string StateId { get; set; }

        public IUserState State { get; set; }

        public IList<IUserRole> Roles { get; set; }

        public IList<IUserToSetting> Settings { get; set; }
    }
}