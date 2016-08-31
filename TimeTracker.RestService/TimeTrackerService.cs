using System.ServiceModel;
using TimeTracker.ServiceBase;

namespace TimeTracker.RestService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TimeTrackerService :
        TimeTrackerServiceBase
    {
    }
}
