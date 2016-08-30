using System.Data;
using TimeTracker.Dal.Entities.Base;

namespace TimeTracker.Dal.Entities
{
    public class TimeRecord :
        Entity<int>
    {
        public int DayRecordId { get; set; }

        public int Hours { get; set; }

        public string Caption { get; set; }

        public bool Deleted { get; set; }

        public TimeRecord()
        {

        }

        public TimeRecord(IDataRecord reader) :
            base(reader)
        {
            DayRecordId = Read<int>(reader, "DayRecordId");
            Hours = Read<int>(reader, "Hours");
            Caption = Read<string>(reader, "Caption");
            Deleted = Read<bool>(reader, "Deleted");
        }
    }
}