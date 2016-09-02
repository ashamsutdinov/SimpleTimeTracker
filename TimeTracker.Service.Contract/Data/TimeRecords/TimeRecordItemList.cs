using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Contract.Data.TimeRecords
{
    [DataContract]
    public class TimeRecordItemList :
        PagedItemList<TimeRecordGroup>
    {
        public TimeRecordItemList() :
            base(0, 0, 0)
        {

        }

        public TimeRecordItemList(IEnumerable<ITimeRecordItem> items, int pageNumber, int pageSize, int totalItems, IUserDataProvider userDataProvider, ITimeRecordDataProvider timeRecordDataProvider) : 
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
                    var preferredHoursSetting = userDataProvider.GetUserSettingValue(group.UserId, "PreferredWorkingHoursPerDay");
                    if (!string.IsNullOrEmpty(preferredHoursSetting))
                    {
                        var warningLimit = int.Parse(preferredHoursSetting);
                        if (group.TotalHours > warningLimit)
                        {
                            group.Warning = true;
                        }
                    }
                    var notes = timeRecordDataProvider.GetTimeRecordNotes(item.Id);
                    group.Notes = notes.Select(n => new TimeRecordGroupNoteItem
                    {
                        Id = n.Id,
                        DateTime = n.DateTime,
                        Text = n.Text,
                        UserName = n.UserName
                    }).ToList();
                    groups[item.Id] = group;
                }
                group.Items.Add(new TimeRecordGroupItem
                {
                    Id = item.TimeRecordId,
                    Hours = item.Hours,
                    Caption = item.Caption,
                    Date = item.Date
                });
                if (string.IsNullOrEmpty(group.UserName))
                {
                    group.UserName = item.UserName;
                }
                Items = groups.Values.ToList();
            }
        }
    }
}