using System.Data.SqlClient;

namespace TimeTracker.Dal.Entities
{
    public class TimeRecord
    {
        public int Id { get; set; }

        public int DayRecordId { get; set; }

        public int Hours { get; set; }

        public string Caption { get; set; }

        public bool Deleted { get; set; }

        public static TimeRecord Read(SqlDataReader reader)
        {
            return new TimeRecord
            {
                Id = (int) reader["Id"],
                DayRecordId = (int) reader["DayRecordId"],
                Hours = (int) reader["Hours"],
                Caption = (string) reader["Caption"],
                Deleted = (bool) reader["Deleted"]
            };
        }
    }
}