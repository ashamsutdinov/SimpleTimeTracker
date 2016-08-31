using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Dal;

using DalDayRecord = TimeTracker.Dal.Entities.DayRecord;
using DtoDayRecord = TimeTracker.Data.Entities.DayRecord;
using DalTimeRecord = TimeTracker.Dal.Entities.TimeRecord;
using DtoTimeRecord = TimeTracker.Data.Entities.TimeRecord;
using DalTimeRecordItem = TimeTracker.Dal.Entities.TimeRecordItem;
using DtoTimeRecordItem = TimeTracker.Data.Entities.TimeRecordItem;
using DalTimeRecordNote = TimeTracker.Dal.Entities.TimeRecordNote;
using DtoTimeRecordNote = TimeTracker.Data.Entities.TimeRecordNote;
using DalTimeRecordNoteItem = TimeTracker.Dal.Entities.TimeRecordNoteItem;
using DtoTimeRecordNoteItem = TimeTracker.Data.Entities.TimeRecordNoteItem;

namespace TimeTracker.Data
{
    public class TimeRecordDataProvider :
        ITimeRecordDataProvider
    {
        private readonly TimeRecordDa _timeRecordDa = new TimeRecordDa();

        public IDayRecord GetUserDayRecordByDate(int userId, DateTime date)
        {
            var dalDayRecord = _timeRecordDa.GetUserDayRecordByDate(userId, date);
            return Mapper.Map<DalDayRecord, IDayRecord>(dalDayRecord);
        }

        public ITimeRecord SaveTimeRecord(int id, int userId, DateTime date, string caption, int hours)
        {
            var dalTimeRecord = _timeRecordDa.SaveTimeRecord(id, userId, date, caption, hours);
            return Mapper.Map<DalTimeRecord, ITimeRecord>(dalTimeRecord);
        }

        public IList<ITimeRecordItem> GetTimeRecords(int? userId, DateTime? fromDate, DateTime? toDate, int pageNumber, int pageSize, out int total)
        {
            var dalRecords = _timeRecordDa.GetTimeRecords(userId, fromDate, toDate, pageNumber, pageSize, out total);
            return dalRecords.Select(Mapper.Map<DalTimeRecordItem, ITimeRecordItem>).ToList();
        }
    }
}
