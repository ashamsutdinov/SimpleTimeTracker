﻿using System.ServiceModel;
using TimeTracker.Contract;
using TimeTracker.Contract.Data;
using TimeTracker.RestService.Utils;

namespace TimeTracker.RestService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TimeTrackingService :
        TimeTrackingServiceBase
    {
        public TimeTrackingService() : 
            base(UnityHelper.Resolve<IUserDataProvider>(), UnityHelper.Resolve<ITimeRecordDataProvider>())
        {
        }
    }
}
