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
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, userId);
            var fromDateParameter = CreateParameter("@FromDate", SqlDbType.Date, fromDate);
            var toDateParameter = CreateParameter("@ToDate", SqlDbType.Date, toDate);
            var pageNumberParameter = CreateParameter("@PageNumber", SqlDbType.Int, pageNumber);
            var pageSizeParameter = CreateParameter("@PageSize", SqlDbType.Int, pageSize);
            var totalParameter = CreateOutputParameter("@Total", SqlDbType.Int);
            var result = ExecuteReader("[dbo].[GetTimeRecords]",
                reader => Read(reader, r => new TimeRecordItem(r)),
                userIdParameter,
                fromDateParameter,
                toDateParameter,
                pageNumberParameter,
                pageSizeParameter,
                totalParameter);
            total = GetOutputValue<int>(totalParameter);
            return result;
        }

        public TimeRecord SaveTimeRecord(int id, int userId, DateTime date, string caption, int hours)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, userId);
            var dateParameter = CreateParameter("@Date", SqlDbType.Date, date);
            var captionParameter = CreateParameter("@Caption", SqlDbType.NVarChar, caption);
            var hoursParameter = CreateParameter("@Hours", SqlDbType.Int, hours);
            var result = ExecuteReader("[dbo].[SaveTimeRecord]",
                reader => ReadSingle(reader, r => new TimeRecord(r)),
                idParameter,
                userIdParameter,
                dateParameter,
                captionParameter,
                hoursParameter);
            return result;
        }

        public TimeRecord GetTimeRecord(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            var result = ExecuteReader("[dbo].[GetTimeRecord]",
                reader => ReadSingle(reader, r => new TimeRecord(r)),
                idParameter);
            return result;
        }

        public int DeleteTimeRecordNote(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            return ExecuteNonQuery("[dbo].[DeleteTimeRecordNote]", idParameter);
        }

        public List<TimeRecordNoteItem> GetTimeRecordNotes(int timeRecordId)
        {
            var timeRecordIdParameter = CreateParameter("@TimeRecordId", SqlDbType.Int, timeRecordId);
            var result = ExecuteReader("[dbo].[GetTimeRecordNotes]",
                reader => Read(reader, r => new TimeRecordNoteItem(r)),
                timeRecordIdParameter);
            return result;
        }

        public TimeRecordNote SaveTimeRecordNote(TimeRecordNote note)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, note.Id);
            var timeRecordIdParameter = CreateParameter("@TimeRecordId", SqlDbType.Int, note.TimeRecordId);
            var userIdParameter = CreateParameter("@UserId", SqlDbType.Int, note.UserId);
            var textParameter = CreateParameter("@Text", SqlDbType.NVarChar, note.Text);
            var result = ExecuteReader("[dbo].[SaveTimeRecordNote]",
                reader => ReadSingle(reader, r => new TimeRecordNote(r)),
                idParameter,
                timeRecordIdParameter,
                userIdParameter,
                textParameter);
            return result;
        }

        public DayRecord GetDayRecord(int id)
        {
            var idParameter = CreateParameter("@Id", SqlDbType.Int, id);
            var result = ExecuteReader("[dbo].[GetDayRecord]",
                reader => ReadSingle(reader, r => new DayRecord(r)),
                idParameter);
            return result;
        }

        public DayRecord GetUserDayRecordByDate(int userId, DateTime date)
        {
            var userIdParameter = CreateParameter("@UserId", SqlDbType.NVarChar, userId);
            var dateParameter = CreateParameter("@Date", SqlDbType.Date, date);
            var result = ExecuteReader("[dbo].[GetUserDayRecordByDate]",
                reader => ReadSingle(reader, r => new DayRecord(r)),
                userIdParameter,
                dateParameter);
            return result;
        }
    }
}
