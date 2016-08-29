using System.Data;

namespace TimeTracker.Dal.Entities.Base
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; set; }

        protected Entity()
        {
            
        }

        protected Entity(IDataRecord reader)
        {
            Id = (TKey)reader["Id"];
        }
    }
}