using System;

namespace TimeTracker.Contract.Data.Entities
{
    public interface IDayRecord : 
        IEntity<int>
    {
        int UserId { get; set; }

        DateTime Date { get; set; }

        int TotalHours { get; set; }
    }
}