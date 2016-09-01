using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    public class TimeRecordItem :
        DayRecord,
        ITimeRecordItem
    {
        public string UserName { get; set; }

        public int TimeRecordId { get; set; }

        public string Caption { get; set; }

        public int Hours { get; set; }
    }
}