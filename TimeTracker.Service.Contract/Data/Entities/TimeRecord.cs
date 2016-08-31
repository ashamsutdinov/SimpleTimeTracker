using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data.Entities
{
    [DataContract]
    public class TimeRecord :
        Entity<int>,
        ITimeRecord
    {
        [DataMember]
        public int DayRecordId { get; set; }

        [DataMember]
        public int Hours { get; set; }

        [DataMember]
        public string Caption { get; set; }

        [DataMember]
        public bool Deleted { get; set; }
    }
}