using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data
{
    [DataContract]
    public class PagedItemList<TItem> :
        ItemList<TItem>
    {
        protected PagedItemList(int pageNumber, int pageSize, int totalItems)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            if (PageSize > 0)
            {
                var fullPages = totalItems/pageSize;
                var extraPage = totalItems%pageSize > 0 ? 1 : 0;
                TotalPages = fullPages + extraPage;
            }
        }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int TotalItems { get; set; }

        [DataMember]
        public int TotalPages { get; set; }

    }
}