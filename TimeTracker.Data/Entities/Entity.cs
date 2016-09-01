using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    public abstract class Entity<TKey> :
        IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}