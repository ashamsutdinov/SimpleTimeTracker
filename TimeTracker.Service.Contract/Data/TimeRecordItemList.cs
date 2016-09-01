using System.Collections.Generic;
using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data
{
    [DataContract]
    public class TimeRecordItemList :
        ItemList<TimeRecordGroup>
    {
        public TimeRecordItemList() :
            base(0, 0, 0)
        {

        }

        public TimeRecordItemList(List<ITimeRecordItem> items, int pageNumber, int pageSize, int totalItems) : 
            base(pageNumber, pageSize, totalItems)
        {
            var groups = new Dictionary<int, TimeRecordGroup>();
            foreach (var item in items)
            {
                TimeRecordGroup group;
                if (!groups.TryGetValue(item.Id, out group))
                {
                    group = new TimeRecordGroup
                    {
                        Id = item.Id,
                        Date = item.Date,
                        TotalHours = item.TotalHours,
                        UserId = item.UserId,
                        Items = new List<TimeRecordGroupItem>()
                    };
                    groups[item.Id] = group;
                }
                group.Items.Add(new TimeRecordGroupItem
                {
                    Id = item.TimeRecordId,
                    Hours = item.Hours,
                    Caption = item.Caption
                });
                if (string.IsNullOrEmpty(group.UserName))
                {
                    group.UserName = item.UserName;
                }
            }
        }
    }
}