using System;
using TimeTracker.Contract.Data.Entities.Base;

namespace TimeTracker.Contract.Data.Entities
{
    public interface ITimeRecordNote :
        IEntity<int>
    {
        int TimeRecordId { get; set; }

        int UserId { get; set; }

        DateTime DateTime { get; set; }

        string Text { get; set; }

        bool Deleted { get; set; }
    }
}