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
            DayRecordId = (int)reader["DayRecordId"];
            Hours = (int)reader["Hours"];
            Caption = (string)reader["Caption"];
            Deleted = (bool)reader["Deleted"];
        }
    }
}