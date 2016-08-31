using System;
using System.Runtime.Serialization;

namespace TimeTracker.ServiceBase.Api
{
    [DataContract]
    public class TimeRecordData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public string Caption { get; set; }

        [DataMember]
        public int Hours { get; set; }
    }
}