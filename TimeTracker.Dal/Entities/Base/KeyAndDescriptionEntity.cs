using System.Data;

namespace TimeTracker.Dal.Entities.Base
{
    public abstract class KeyAndDescriptionEntity : 
        Entity<string>
    {
        public string Description { get; set; }

        protected KeyAndDescriptionEntity()
        {
            
        }

        protected KeyAndDescriptionEntity(IDataRecord reader) : 
            base(reader)
        {
            Description = Read<string>(reader, "Description");
        }
    }
}