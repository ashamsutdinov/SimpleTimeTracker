namespace TimeTracker.Contract.Data.Entities
{
    public interface ITimeRecordItem : 
        IDayRecord
    {
        string UserName { get; set; }

        int TimeRecordId { get; set; }

        string Caption { get; set; }

        int Hours { get; set; }
    }
}