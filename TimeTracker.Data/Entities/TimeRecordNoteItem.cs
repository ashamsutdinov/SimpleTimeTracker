using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    public class TimeRecordNoteItem :
        TimeRecordNote,
        ITimeRecordNoteItem
    {
        public string UserName { get; set; }
    }
}