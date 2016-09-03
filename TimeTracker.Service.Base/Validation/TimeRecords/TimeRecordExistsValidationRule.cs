using System;
using TimeTracker.Contract.Data;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.TimeRecords;

namespace TimeTracker.Service.Base.Validation.TimeRecords
{
    internal class TimeRecordExistsValidationRule :
        AnonymousRequestValidationRule<TimeRecordData>
    {
        private readonly ITimeRecordDataProvider _timeRecordDataProvider;

        public TimeRecordExistsValidationRule(ITimeRecordDataProvider timeRecordDataProvider) : 
            base(true)
        {
            _timeRecordDataProvider = timeRecordDataProvider;
        }

        protected override void EvaluateAnonymous(Request<TimeRecordData> request)
        {
            if (request.Data.Id > 0)
            {
                var timeRecord = _timeRecordDataProvider.GetTimeRecord(request.Data.Id);
                if (timeRecord == null)
                {
                    throw new InvalidOperationException("Time record was not found");
                }
            }
        }
    }
}