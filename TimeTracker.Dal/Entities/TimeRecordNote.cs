using System;
using System.Data;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class TimeRecordNote :
        Entity<int>
    {
        public int TimeRecordId { get; set; }

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
            TimeRecordId = Read<int>(reader, "TimeRecordId");
            UserId = Read<int>(reader, "userId");
            DateTime = Read<DateTime>(reader, "DateTime");
            Text = Read<string>(reader, "Text");
            Deleted = Read<bool>(reader, "Deleted");
        }
    }
}