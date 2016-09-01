using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data
{
    [DataContract]
    public class ItemList<TItem>
    {
        [DataMember]
        public List<TItem> Items { get; set; } 
    }
}