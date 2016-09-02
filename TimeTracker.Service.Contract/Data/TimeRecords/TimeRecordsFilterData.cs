using System;
using System.Runtime.Serialization;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Contract.Data.TimeRecords
{
    [DataContract]
    public class TimeRecordsFilterData : 
        PageData
    {
        [DataMember]
        public DateTime From { get; set; }

        [DataMember]
        public DateTime To { get; set; }

        [DataMember]
        public bool LoadAllUsers { get; set; }
    }
}