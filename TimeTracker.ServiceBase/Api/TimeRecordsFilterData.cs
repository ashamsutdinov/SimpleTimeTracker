using System;
using System.Runtime.Serialization;

namespace TimeTracker.ServiceBase.Api
{
    [DataContract]
    public class TimeRecordsFilterData
    {
        [DataMember]
        public DateTime From { get; set; }

        [DataMember]
        public DateTime To { get; set; }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public bool LoadAllUsers { get; set; }
    }
}