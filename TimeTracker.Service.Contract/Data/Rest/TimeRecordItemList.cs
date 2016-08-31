using System.Runtime.Serialization;
using TimeTracker.Service.Contract.Data.Entities;

namespace TimeTracker.Service.Contract.Data.Rest
{
    [DataContract]
    public class TimeRecordItemList :
        ItemList<TimeRecordItem>
    {
        
    }
}