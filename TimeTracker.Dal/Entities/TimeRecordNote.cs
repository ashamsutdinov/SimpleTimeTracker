using System;
using System.Data.SqlClient;

namespace TimeTracker.Dal.Entities
{
    public class TimeRecordNote
    {
        public int Id { get; set; }

        public int TimeRecordId { get; set; }

        public int UserId { get; set; }

        public DateTime DateTime { get; set; }

        public string Text { get; set; }

        public bool Deleted { get; set; }

        public static TimeRecordNote Read(SqlDataReader reader)
        {
            return new TimeRecordNote
            {
                Id = (int) reader["Id"],
                TimeRecordId = (int) reader["TimeRecordId"],
                UserId = (int) reader["UserId"],
                DateTime = (DateTime) reader["DateTime"],
                Text = (string) reader["Text"],
                Deleted = (bool) reader["Deleted"]
            };
        }
    }
}