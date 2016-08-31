using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data.Entities
{
    [DataContract]
    public class TimeRecordNoteItem :
        TimeRecordNote,
        ITimeRecordNoteItem
    {
        [DataMember]
        public string UserName { get; set; }
    }
}