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
            UserName = (string)reader["UserName"];
            TimeRecordId = (int)reader["TimeRecordId"];
            Caption = (string)reader["Caption"];
            Hours = (int)reader["Hours"];
        }
    }
}