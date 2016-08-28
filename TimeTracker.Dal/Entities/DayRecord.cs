using System;
using System.Data.SqlClient;

namespace TimeTracker.Dal.Entities
{
    public class DayRecord
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public int TotalHours { get; set; }

        public static DayRecord Read(SqlDataReader reader)
        {
            return new DayRecord
            {
                Id = (int) reader["Id"],
                UserId = (int) reader["UserId"],
                Date = (DateTime) reader["Date"],
                TotalHours = (int) reader["TotalHours"]
            };
        }
    }
}
