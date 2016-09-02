using System;
using System.Collections.Generic;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Contract.Data
{
    public interface ITimeRecordDataProvider
    {
        IDayRecord GetUserDayRecordByDate(int userId, DateTime date);

        ITimeRecord SaveTimeRecord(int id, int userId, DateTime date, string caption, int hours);

        ITimeRecord GetTimeRecord(int id);

        IDayRecord GetDayRecord(int id);

        IList<ITimeRecordItem> GetTimeRecords(int? userId, DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize, out int total);

        int DeleteTimeRecord(int timeRecordId);

        ITimeRecordNote GetTimeRecordNote(int id);

        IList<ITimeRecordNoteItem> GetTimeRecordNotes(int dayRecordId);
        ITimeRecordNote PrepareTimeRecordNote(int dayRecordId, int userId, string text);

        ITimeRecordNote SaveTimeRecordNote(ITimeRecordNote note);

        int DeleteTimeRecordNote(int timeRecordNoteId);
    }
}
