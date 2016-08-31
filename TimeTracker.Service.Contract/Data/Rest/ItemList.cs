using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.Rest
{
    [DataContract]
    public class ItemList<TItem>
    {
        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int TotalItems { get; set; }

        [DataMember]
        public int TotalPages { get; set; }

        [DataMember]
        public List<TItem> Items { get; set; }
       
    }
}