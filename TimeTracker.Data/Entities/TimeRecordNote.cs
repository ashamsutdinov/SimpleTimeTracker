using System;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    public class TimeRecordNote :
        Entity<int>,
        ITimeRecordNote
    {
        public int DayRecordId { get; set; }

        public int UserId { get; set; }

        public DateTime DateTime { get; set; }

        public string Text { get; set; }

        public bool Deleted { get; set; }
    }
}