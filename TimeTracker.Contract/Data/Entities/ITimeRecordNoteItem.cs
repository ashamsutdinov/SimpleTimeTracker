namespace TimeTracker.Contract.Data.Entities
{
    public interface ITimeRecordNoteItem :
        ITimeRecordNote
    {
        string UserName { get; set; }
    }
}