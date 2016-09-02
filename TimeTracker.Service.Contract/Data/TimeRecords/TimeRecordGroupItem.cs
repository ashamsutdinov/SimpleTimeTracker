using System;
using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.TimeRecords
{
    [DataContract]
    public class TimeRecordGroupItem
    {
        //Equal to TimeRecordId
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Caption {get; set; }

        [DataMember]
        public int Hours { get; set; }

        [DataMember]
        public DateTime Date { get; set; }
    }
}