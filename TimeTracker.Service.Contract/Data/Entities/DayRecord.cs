using System;
using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data.Entities
{
    [DataContract]
    public class DayRecord :
        Entity<int>,
        IDayRecord
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public int TotalHours { get; set; }
    }
}
