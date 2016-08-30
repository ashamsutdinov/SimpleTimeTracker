using System.Data;

namespace TimeTracker.Dal.Entities
{
    public class TimeRecordItem :
        DayRecord
    {

        public string UserName { get; set; }

        public int TimeRecordId { get; set; }

        public string Caption { get; set; }

        public int Hours { get; set; }

        public TimeRecordItem(IDataRecord reader) :
            base(reader)
        {
            UserName = Read<string>(reader, "UserName");
            TimeRecordId = Read<int>(reader, "TimeRecordId");
            Caption = Read<string>(reader, "Caption");
            Hours = Read<int>(reader, "Hours");
        }
    }
}