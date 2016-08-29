using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TimeTracker.Dal.Entities;
using TimeTracker.Dal.Utils;

namespace TimeTracker.Dal
{
    public class TimeRecordDa :
        BaseDa
    {
        public int DeleteTimeRecord(int id)
        {
            var idParameter = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            return ExecuteNonQuery("[dbo].[DeleteTimeRecord]", idParameter);
        }

        public IList<TimeRecordItem> GetTimeRecords(int? userId, DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize, out int total)
        {
            IDbCommand outCommand;
            var userIdParameter = new SqlParameter("@UserId", SqlDbType.Int) { Value = userId.HasValue ? (object)userId.Value : DBNull.Value };
            var fromDateParameter = new SqlParameter("@FromDate", SqlDbType.Date) { Value = fromDate.HasValue ? (object)fromDate.Value : DBNull.Value };
            var toDateParameter = new SqlParameter("@ToDate", SqlDbType.Date) { Value = toDate.HasValue ? (object)toDate.Value : DBNull.Value };
            var pageNumberParameter = new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber };
            var pageSizeParameter = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
            var reader = ExecuteReader("[dbo].[GetTimeRecords]", out outCommand, userIdParameter, fromDateParameter, toDateParameter, pageNumberParameter, pageSizeParameter);
            var result = new List<TimeRecordItem>();
            while (reader.Read())
            {
                result.Add(new TimeRecordItem(reader));
            }
            var outParameter = outCommand.Parameters["@Total"] as SqlParameter;
            total = outParameter != null ? (int)outParameter.Value : 0;
            return result;
        }

        public TimeRecord SaveTimeRecord(int id, int userId, DateTime date, string caption, int hours)
        {
            var idParameter = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            var userIdParameter = new SqlParameter("@UserId", SqlDbType.Int) { Value = userId };
            var dateParameter = new SqlParameter("@Date", SqlDbType.Date) { Value = date };
            var captionParameter = new SqlParameter("@Caption", SqlDbType.NVarChar) { Value = caption };
            var hoursParameter = new SqlParameter("@Hours", SqlDbType.Int) { Value = hours };
            var reader = ExecuteReader("[dbo].[SaveTimeRecord]", idParameter, userIdParameter, dateParameter, captionParameter, hoursParameter);
            if (reader.Read())
            {
                return new TimeRecord(reader);
            }
            return null;
        }
    }
}
