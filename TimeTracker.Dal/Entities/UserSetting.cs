using System.Data;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class UserSetting : 
        KeyAndDescriptionEntity
    {
        public UserSetting()
        {
            
        }

        public UserSetting(IDataRecord reader) :
            base(reader)
        {
            
        }
    }
}