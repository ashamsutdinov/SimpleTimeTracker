using System;
using System.Collections.Generic;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Contract.Data
{
    public interface ITimeRecordDataProvider
    {
        IDayRecord GetUserDayRecordByDate(int userId, DateTime date);

        ITimeRecord SaveTimeRecord(int id, int userId, DateTime date, string caption, int hours);

        IList<ITimeRecordItem> GetTimeRecords(int? userId, DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize, out int total);
    }
}
