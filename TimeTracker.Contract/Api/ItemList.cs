using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TimeTracker.Contract.Api
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
        public IList<TItem> Items { get; set; }

        [OnSerializing]
        void OnSerializing(StreamingContext ctx)
        {

        }
    }
}