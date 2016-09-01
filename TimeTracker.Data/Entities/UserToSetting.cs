using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    public class UserToSetting :
        UserSetting,
        IUserToSetting
    {
        public string Value { get; set; }
    }
}