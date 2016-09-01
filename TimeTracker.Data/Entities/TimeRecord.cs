using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    public class TimeRecord :
        Entity<int>,
        ITimeRecord
    {
        public int DayRecordId { get; set; }

        public int Hours { get; set; }

        public string Caption { get; set; }

        public bool Deleted { get; set; }
    }
}