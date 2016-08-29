using System.Data;

namespace TimeTracker.Dal.Entities.Base
{
    public abstract class IdAndDescriptionEntity : 
        Entity<string>
    {
        public string Description { get; set; }

        protected IdAndDescriptionEntity()
        {
            
        }

        protected IdAndDescriptionEntity(IDataRecord reader) : 
            base(reader)
        {
            Description = (string) reader["Description"];
        }
    }
}