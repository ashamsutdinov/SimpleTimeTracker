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

        public int DeleteTimeRecord(int timeRecordId)
        {
            return _timeRecordDa.DeleteTimeRecord(timeRecordId);
        }

        public ITimeRecordNote GetTimeRecordNote(int id)
        {
            var dalNote = _timeRecordDa.GetTimeRecordNote(id);
            return Mapper.Map<DalTimeRecordNote, ITimeRecordNote>(dalNote);
        }

        public IList<ITimeRecordNoteItem> GetTimeRecordNotes(int dayRecordId)
        {
            var dalNotes = _timeRecordDa.GetTimeRecordNotes(dayRecordId);
            return dalNotes.Select(Mapper.Map<DalTimeRecordNoteItem, ITimeRecordNoteItem>).ToList();
        }

        public ITimeRecordNote PrepareTimeRecordNote(int dayRecordId, int userId, string text)
        {
            return new DtoTimeRecordNote
            {
                DayRecordId = dayRecordId,
                DateTime = DateTime.UtcNow,
                UserId = userId,
                Text = text
            };
        }

        public ITimeRecordNote SaveTimeRecordNote(ITimeRecordNote note)
        {
            var dalNote = Mapper.Map<ITimeRecordNote, DalTimeRecordNote>(note);
            dalNote = _timeRecordDa.SaveTimeRecordNote(dalNote);
            return Mapper.Map<DalTimeRecordNote, ITimeRecordNote>(dalNote);
        }

        public ITimeRecordNote SaveTimeRecordNote(int id, int dayRecordId, int userId, string text)
        {
            var note = PrepareTimeRecordNote(dayRecordId, userId, text);
            note.Id = id;
            return SaveTimeRecordNote(note);
        }

        public int DeleteTimeRecordNote(int timeRecordNoteId)
        {
            return _timeRecordDa.DeleteTimeRecordNote(timeRecordNoteId);
        }
    }
}
