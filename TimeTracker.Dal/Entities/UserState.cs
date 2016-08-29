using System.Data;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class UserState :
        KeyAndDescriptionEntity
    {
        public UserState()
        {

        }

        public UserState(IDataRecord reader) :
            base(reader)
        {

        }
    }
}