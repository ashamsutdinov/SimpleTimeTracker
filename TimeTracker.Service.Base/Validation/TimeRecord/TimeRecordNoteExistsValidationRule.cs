using System;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Validation.TimeRecord
{
    internal class TimeRecordNoteExistsValidationRule :
        RequestValidationRule<TimeRecordNoteData>
    {
        private readonly ITimeRecordDataProvider _timeRecordDataProvider;

        public TimeRecordNoteExistsValidationRule(ITimeRecordDataProvider timeRecordDataProvider, params string[] requiredRoles)
        {
            _timeRecordDataProvider = timeRecordDataProvider;
        }

        protected override void EvaluateInternal(IUser user, IUserSession userSession, Request<TimeRecordNoteData> request)
        {
            if (request.Data.Id == 0)
            {
                return;
            }

            var timeRecordNote = _timeRecordDataProvider.GetTimeRecordNote(request.Data.Id);
            if (timeRecordNote == null)
            {
                throw new InvalidOperationException("Time record note does not exists");
            }
        }
    }
}