using System;
using System.Runtime.Serialization;

namespace TimeTracker.Service.Contract.Data
{
    [DataContract]
    public class TimeRecordGroupNoteItem
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public string UserName { get; set; }
    }
}