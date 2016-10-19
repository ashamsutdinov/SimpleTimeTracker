using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data.TimeRecords
{
    [DataContract]
    public class TimeRecordNoteData
    {
        public int Id { get; set; }

        [DataMember]
        public int DayRecordId { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}