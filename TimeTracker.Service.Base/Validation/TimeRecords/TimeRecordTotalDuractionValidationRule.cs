using System;
using TimeTracker.Contract.Data;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.TimeRecords;

namespace TimeTracker.Service.Base.Validation.TimeRecords
{
    internal class TimeRecordTotalDuractionValidationRule :
        AnonymousRequestValidationRule<TimeRecordData>
    {
        private readonly ITimeRecordDataProvider _timeRecordDataProvider;

        public TimeRecordTotalDuractionValidationRule(ITimeRecordDataProvider timeRecordDataProvider)
        {
            _timeRecordDataProvider = timeRecordDataProvider;
        }

        protected override void EvaluateAnonymous(Request<TimeRecordData> request)
        {
            if (request.Data.Id == 0)
            {
                return;
            }
            var timeRecord = _timeRecordDataProvider.GetTimeRecord(request.Data.Id);
            var dayRecord = _timeRecordDataProvider.GetDayRecord(timeRecord.DayRecordId);
            var substract = 0;
            if (dayRecord.Date != request.Data.Date)
            {
                dayRecord = _timeRecordDataProvider.GetUserDayRecordByDate(dayRecord.UserId, request.Data.Date);
                if (dayRecord == null)
                {
                    //new day record will be created
                    return;
                }
            }
            else
            {
                substract = timeRecord.Hours;
            }
            var duration = dayRecord.TotalHours - substract + request.Data.Hours;
            if (duration > 24)
            {
                throw new InvalidOperationException("The number of working hours per day may not exceed 24 hours");
            }

        }
    }
}