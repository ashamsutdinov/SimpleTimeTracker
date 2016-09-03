using System;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.TimeRecords;

namespace TimeTracker.Service.Base.Validation.TimeRecords
{
    internal class TimeRecordNonZeroDurationValidationRule :
        AnonymousRequestValidationRule<TimeRecordData>
    {
        public TimeRecordNonZeroDurationValidationRule() : 
            base(true)
        {

        }

        protected override void EvaluateAnonymous(Request<TimeRecordData> request)
        {
            if (request.Data.Hours <= 0)
            {
                throw new InvalidOperationException("Duration required");
            }
        }
    }
}