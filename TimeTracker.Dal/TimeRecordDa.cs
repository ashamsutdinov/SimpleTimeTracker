using System;
using System.Collections.Generic;
using System.Data;
using TimeTracker.Dal.Entities;
using TimeTracker.Dal.Utils;

namespace TimeTracker.Dal
{
    public class TimeRecordDa :
        BaseDa
    {
        public int DeleteTimeRecord(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            return ExecuteNonQuery("[dbo].[DeleteTimeRecord]", idParameter);
        }

        public IList<TimeRecordItem> GetTimeRecords(int? userId, DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize, out int total)
        {
            IDbCommand outCommand;
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, userId);
            var fromDateParameter = CreateParameter("@FromDate", SqlDbType.Date, fromDate);
            var toDateParameter = CreateParameter("@ToDate", SqlDbType.Date, toDate);
            var pageNumberParameter = CreateParameter("@PageNumber", SqlDbType.Int, pageNumber);
            var pageSizeParameter = CreateParameter("@PageSize", SqlDbType.Int, pageSize);
            var reader = ExecuteReader("[dbo].[GetTimeRecords]", out outCommand, userIdParameter, fromDateParameter, toDateParameter, pageNumberParameter, pageSizeParameter);
            var result = Read(reader, r => new TimeRecordItem(r));
            total = GetOutputValue<int>(outCommand, "@Total");
            return result;
        }

        public TimeRecord SaveTimeRecord(int id, int userId, DateTime date, string caption, int hours)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, userId);
            var dateParameter = CreateParameter("@Date", SqlDbType.Date, date);
            var captionParameter = CreateParameter("@Caption", SqlDbType.NVarChar, caption);
            var hoursParameter = CreateParameter("@Hours", SqlDbType.Int, hours);
            var reader = ExecuteReader("[dbo].[SaveTimeRecord]", idParameter, userIdParameter, dateParameter, captionParameter, hoursParameter);
            return ReadSingle(reader, r => new TimeRecord(r));
        }

        public TimeRecord GetTimeRecord(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            var reader = ExecuteReader("[dbo].[GetTimeRecord]", idParameter);
            return ReadSingle(reader, r => new TimeRecord(r));
        }

        public int DeleteTimeRecordNote(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            return ExecuteNonQuery("[dbo].[DeleteTimeRecordNote]", idParameter);
        }

        public List<TimeRecordNoteItem> GetTimeRecordNotes(int timeRecordId)
        {
            var timeRecordIdParameter = CreateParameter("@TimeRecordId", SqlDbType.Int, timeRecordId);
            var reader = ExecuteReader("[dbo].[GetTimeRecordNotes]", timeRecordIdParameter);
            return Read(reader, r => new TimeRecordNoteItem(r));
        }

        public TimeRecordNote SaveTimeRecordNote(TimeRecordNote note)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, note.Id);
            var timeRecordIdParameter = CreateParameter("@TimeRecordId", SqlDbType.Int, note.TimeRecordId);
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, note.UserId);
            var textParameter = CreateParameter("@Text", SqlDbType.NVarChar, note.Text);
            var reader = ExecuteReader("[dbo].[SaveTimeRecordNote]", idParameter, timeRecordIdParameter, userIdParameter,textParameter);
            return ReadSingle(reader, r => new TimeRecordNote(r));
        }

        public DayRecord GetDayRecord(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            var reader = ExecuteReader("[dbo].[GetDayRecord]", idParameter);
            return ReadSingle(reader, r => new DayRecord(r));
        }
    }
}
