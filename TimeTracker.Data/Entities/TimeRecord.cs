using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Data.Entities
{
    [DataContract]
    public class TimeRecord :
        ITimeRecord
    {
        [DataMember]
        public int Id { get; set; }

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