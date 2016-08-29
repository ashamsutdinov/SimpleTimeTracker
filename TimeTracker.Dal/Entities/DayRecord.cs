using System;
using System.Data;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class DayRecord :
        Entity<int>
    {
        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public int TotalHours { get; set; }

        public DayRecord()
        {

        }

        public DayRecord(IDataRecord reader) :
            base(reader)
        {
            UserId = (int)reader["UserId"];
            Date = (DateTime)reader["Date"];
            TotalHours = (int)reader["TotalHours"];
        }
    }
}
