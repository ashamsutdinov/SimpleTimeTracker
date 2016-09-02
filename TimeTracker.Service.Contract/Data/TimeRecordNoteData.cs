using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data
{
    [DataContract]
    public class TimeRecordNoteData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int DayRecordId { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}