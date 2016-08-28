using System.Data.SqlClient;

namespace TimeTracker.Dal.Entities
{
    public class UserToSetting :
        UserSetting
    {
        public string Value { get; set; }

        public UserToSetting(UserSetting baseObject)
        {
            Id = baseObject.Id;
            Description = baseObject.Description;
        }

        public new static UserToSetting Read(SqlDataReader reader)
        {
            return new UserToSetting(UserSetting.Read(reader))
            {
                Value = (string)reader["Value"]
            };
        }
    }
}