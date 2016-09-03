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
            var hasManagementPriveleges = user.Roles.Select(r => r.Id).Intersect(_requiredRoles).Any();
            if (request.Data.Id > 0)
            {
                var timeRecordNote = _timeRecordDataProvider.GetTimeRecordNote(request.Data.Id);
                if (timeRecordNote.UserId != user.Id && !hasManagementPriveleges)
                {
                    throw new UnauthorizedAccessException("User is not allowed to perform manipulations with time record notes posted by another users");
                }
            }
            var dayRecord = _timeRecordDataProvider.GetDayRecord(request.Data.DayRecordId);
            if (dayRecord.UserId != user.Id && !hasManagementPriveleges)
            {
                throw new UnauthorizedAccessException("User does not have enough rights to perform manipulations with user's time record notes");
            }
        }
    }
}