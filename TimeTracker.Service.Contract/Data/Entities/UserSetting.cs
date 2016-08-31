using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data.Entities
{
    [DataContract]
    public class UserSetting :
        KeyAndDescriptionEntity,
        IUserSetting
    {

    }
}