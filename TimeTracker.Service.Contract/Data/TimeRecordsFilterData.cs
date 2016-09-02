using System;
using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data
{
    [DataContract]
    public class TimeRecordsFilterData : 
        PageRequest
    {
        [DataMember]
        public DateTime From { get; set; }

        [DataMember]
        public DateTime To { get; set; }

        [DataMember]
        public bool LoadAllUsers { get; set; }
    }
}