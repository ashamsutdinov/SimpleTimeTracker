using System.Collections.Generic;
using TimeTracker.Contract.Data.Entities.Base;

namespace TimeTracker.Contract.Data.Entities
{
    public interface IUser :
        IEntity<int>
    {
        string Login { get; set; }

        string PasswordHash { get; set; }

        string PasswordSalt { get; set; }

        string Name { get; set; }

        string StateId { get; set; }

        IUserState State { get; set; }

        IList<IUserRole> Roles { get; set; }

        IList<IUserToSetting> Settings { get; set; }
    }
}