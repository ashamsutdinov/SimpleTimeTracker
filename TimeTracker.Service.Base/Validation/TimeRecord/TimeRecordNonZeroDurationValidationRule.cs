using System;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Validation.TimeRecord
{
    internal class TimeRecordNonZeroDurationValidationRule :
        AnonymousRequestValidationRule<TimeRecordData>
    {
        protected override void EvaluateAnonymous(Request<TimeRecordData> request)
        {
            if (request.Data.Hours <= 0)
            {
                throw new InvalidOperationException("Duration required");
            }
        }
    }
}