using System;
using System.Linq;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Validation.TimeRecord
{
    internal class TimeRecordNoteEditRightsValidationRule :
        RequestValidationRule<TimeRecordNoteData>
    {
        private readonly string[] _requiredRoles;

        private readonly ITimeRecordDataProvider _timeRecordDataProvider;

        public TimeRecordNoteEditRightsValidationRule(ITimeRecordDataProvider timeRecordDataProvider, params string[] requiredRoles)
        {
            _timeRecordDataProvider = timeRecordDataProvider;
            _requiredRoles = requiredRoles;
        }

        protected override void EvaluateInternal(IUser user, IUserSession userSession, Request<TimeRecordNoteData> request)
        {
            var dayRecord = _timeRecordDataProvider.GetDayRecord(request.Data.DayRecordId);
            if (dayRecord.UserId != user.Id && user.Roles.Select(r => r.Id).Intersect(_requiredRoles).Any())
            {
                throw new UnauthorizedAccessException("User does not have enough rights to perform manipulations with user's time record notes");
            }
        }
    }
}