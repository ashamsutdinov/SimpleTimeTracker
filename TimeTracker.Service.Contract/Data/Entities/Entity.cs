using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data.Entities
{
    [DataContract]
    public abstract class Entity<TKey> :
        IEntity<TKey>
    {
        [DataMember]
        public TKey Id { get; set; }
    }
}