using System;
using System.Linq;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.TimeRecords;

namespace TimeTracker.Service.Base.Validation.TimeRecords
{
    internal class TimeRecordEditRightsValidationRule :
        RequestValidationRule<TimeRecordData>
    {
        private readonly string[] _requiredRoles;

        private readonly ITimeRecordDataProvider _timeRecordDataProvider;

        public TimeRecordEditRightsValidationRule(ITimeRecordDataProvider timeRecordDataProvider, params string[] requiredRoles)
        {
            _timeRecordDataProvider = timeRecordDataProvider;
            _requiredRoles = requiredRoles;
        }

        protected override void EvaluateInternal(IUser user, IUserSession userSession, Request<TimeRecordData> request)
        {
            if (request.Data.Id == 0)
            {
                //this is new record 
                return;
            }
            var existingTimeRecprd = _timeRecordDataProvider.GetTimeRecord(request.Data.Id);
            var existingDayRecord = _timeRecordDataProvider.GetDayRecord(existingTimeRecprd.DayRecordId);
            if (existingDayRecord.UserId != user.Id && !user.Roles.Select(r => r.Id).Intersect(_requiredRoles).Any())
            {
                throw new UnauthorizedAccessException("User does not have enough rights to perform manipulations with user's time records");
            }
        }
    }
}