using System.Data.SqlClient;

namespace TimeTracker.Dal.Entities
{
    public class TimeRecordItem :
        DayRecord
    {
        public TimeRecordItem(DayRecord baseRecord)
        {
            Id = baseRecord.Id;
            UserId = baseRecord.UserId;
            Date = baseRecord.Date;
            TotalHours = baseRecord.TotalHours;
        }

        public string UserName { get; set; }

        public int TimeRecordId { get; set; }

        public string Caption { get; set; }

        public int Hours { get; set; }

        public new static TimeRecordItem Read(SqlDataReader reader)
        {
            var result = new TimeRecordItem(DayRecord.Read(reader))
            {
                UserName = (string) reader["UserName"],
                TimeRecordId = (int) reader["TimeRecordId"],
                Caption = (string) reader["Caption"],
                Hours = (int) reader["Hours"]
            };
            return result;
        }
    }
}