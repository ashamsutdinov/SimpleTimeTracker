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
            TimeRecordId = (int)reader["TimeRecordId"];
            UserId = (int)reader["UserId"];
            DateTime = (DateTime)reader["DateTime"];
            Text = (string)reader["Text"];
            Deleted = (bool)reader["Deleted"];
        }
    }
}