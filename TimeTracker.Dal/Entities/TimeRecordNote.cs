using System;
using System.Data;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class TimeRecordNote :
        Entity<int>
    {
        public int DayRecordId { get; set; }

        public int UserId { get; set; }

        public DateTime DateTime { get; set; }

        public string Text { get; set; }

        public bool Deleted { get; set; }

        public TimeRecordNote()
        {

        }

        public TimeRecordNote(IDataRecord reader) :
            base(reader)
        {
            DayRecordId = Read<int>(reader, "DayRecordId");
            UserId = Read<int>(reader, "UserId");
            DateTime = Read<DateTime>(reader, "DateTime");
            Text = Read<string>(reader, "Text");
            Deleted = Read<bool>(reader, "Deleted");
        }
    }
}