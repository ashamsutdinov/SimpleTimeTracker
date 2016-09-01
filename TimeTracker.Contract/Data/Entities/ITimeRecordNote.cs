using System;

namespace TimeTracker.Contract.Data.Entities
{
    public interface ITimeRecordNote :
        IEntity<int>
    {
        int DayRecordId { get; set; }

        int UserId { get; set; }

        DateTime DateTime { get; set; }

        string Text { get; set; }

        bool Deleted { get; set; }
    }
}