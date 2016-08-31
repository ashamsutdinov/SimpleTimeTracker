using System.ServiceModel;
using TimeTracker.Service.Base;

namespace TimeTracker.RestService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TimeTrackerService :
        TimeTrackerServiceBase
    {
    }
}
