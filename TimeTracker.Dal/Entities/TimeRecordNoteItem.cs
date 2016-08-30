using System.Data;

namespace TimeTracker.Dal.Entities
{
    public class TimeRecordNoteItem :
        TimeRecordNote
    {
        public string UserName { get; set; }

        public TimeRecordNoteItem()
        {
            
        }

        public TimeRecordNoteItem(IDataRecord reader) :
            base(reader)
        {
            UserName = (string) reader["UserName"];
        }
    }
}