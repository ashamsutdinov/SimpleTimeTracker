using System.Collections.Generic;
using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data.Entities
{
    [DataContract]
    public class User :
        Entity<int>,
        IUser
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }

        [DataMember]
        public string PasswordSalt { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string StateId { get; set; }

        [DataMember]
        public IUserState State { get; set; }

        [DataMember]
        public IList<IUserRole> Roles { get; set; }

        [DataMember]
        public IList<IUserToSetting> Settings { get; set; }
    }
}