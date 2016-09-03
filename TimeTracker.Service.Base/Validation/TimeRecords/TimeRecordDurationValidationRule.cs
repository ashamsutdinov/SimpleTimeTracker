using System;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.TimeRecords;

namespace TimeTracker.Service.Base.Validation.TimeRecords
{
    internal class TimeRecordDurationValidationRule :
        AnonymousRequestValidationRule<TimeRecordData>
    {
        public TimeRecordDurationValidationRule() : 
            base(true)
        {
        }

        protected override void EvaluateAnonymous(Request<TimeRecordData> request)
        {
            if (request.Data.Hours > 24)
            {
                throw new InvalidOperationException("The number of working hours per day may not exceed 24 hours");
            }
        }
    }
}