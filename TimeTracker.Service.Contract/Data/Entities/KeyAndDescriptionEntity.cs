using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Entities
{
    [DataContract]
    public class KeyAndDescriptionEntity :
        Entity<string>
    {
        [DataMember]
        public string Description { get; set; }
    }
}