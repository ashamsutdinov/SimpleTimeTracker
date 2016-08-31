namespace TimeTracker.Contract.Data.Entities
{
    public interface ITimeRecord :
        IEntity<int>
    {
        int DayRecordId { get; set; }

        int Hours { get; set; }

        string Caption { get; set; }

        bool Deleted { get; set; }
    }
}