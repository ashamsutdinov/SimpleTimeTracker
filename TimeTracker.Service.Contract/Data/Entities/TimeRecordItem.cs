using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data.Entities
{
    [DataContract]
    public class TimeRecordItem :
        DayRecord,
        ITimeRecordItem
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int TimeRecordId { get; set; }

        [DataMember]
        public string Caption { get; set; }

        [DataMember]
        public int Hours { get; set; }
    }
}