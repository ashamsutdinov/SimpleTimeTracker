using System.Runtime.Serialization;
using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.ServiceBase.Entities
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