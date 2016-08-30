using System.Data;

namespace TimeTracker.Dal.Entities
{
    public class UserToSetting :
        UserSetting
    {
        public string Value { get; set; }

        public UserToSetting()
        {

        }

        public UserToSetting(IDataRecord reader) :
            base(reader)
        {
            Value = Read<string>(reader, "Value");
        }
    }
}