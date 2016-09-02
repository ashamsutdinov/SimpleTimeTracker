using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Base
{
    [DataContract]
    public abstract class ItemList<TItem>
    {
        [DataMember]
        public List<TItem> Items { get; set; } 
    }
}