using System.ServiceModel;
using TimeTracker.Contract;

namespace TimeTracker.RestService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TimeTrackerService :
        TimeTrackerServiceBase
    {
    }
}
