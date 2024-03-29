using System;
using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.TimeRecords
{
    [DataContract]
    public class TimeRecordData 
    {
        public int Id { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public string Caption { get; set; }

        [DataMember]
        public int Hours { get; set; }
    }
}