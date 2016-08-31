using System;
using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data.Entities
{
    [DataContract]
    public class TimeRecordNote :
        Entity<int>,
        ITimeRecordNote
    {
        [DataMember]
        public int TimeRecordId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public bool Deleted { get; set; }
    }
}