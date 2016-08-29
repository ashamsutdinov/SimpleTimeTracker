using System.Data;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class UserState :
        IdAndDescriptionEntity
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