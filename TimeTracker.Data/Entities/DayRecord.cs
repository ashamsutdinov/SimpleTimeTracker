using System;
using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    [DataContract]
    public class DayRecord :
        IDayRecord
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public int TotalHours { get; set; }
    }
}
