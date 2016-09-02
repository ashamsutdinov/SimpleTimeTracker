using System;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Validation.TimeRecord
{
    internal class TimeRecordDurationValidationRule :
        AnonymousRequestValidationRule<TimeRecordData>
    {
        protected override void EvaluateAnonymous(Request<TimeRecordData> request)
        {
            if (request.Data.Hours > 24)
            {
                throw new InvalidOperationException("The number of working hours per day may not exceed 24 hours");
            }
        }
    }
}