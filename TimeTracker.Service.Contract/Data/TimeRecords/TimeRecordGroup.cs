using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.TimeRecords
{
    [DataContract]
    public class TimeRecordGroup
    {
        //Equal to DayRecordId
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public int TotalHours { get; set; }

        //if greater than PreferredWorkingHoursPerDay
        [DataMember]
        public bool Warning { get; set; }

        [DataMember]
        public List<TimeRecordGroupItem> Items { get; set; }

        [DataMember]
        public List<TimeRecordGroupNoteItem> Notes { get; set; } 
    }
}