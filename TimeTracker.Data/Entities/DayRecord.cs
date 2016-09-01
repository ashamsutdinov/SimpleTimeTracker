using System;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    public class DayRecord :
        Entity<int>,
        IDayRecord
    {
        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public int TotalHours { get; set; }
    }
}
