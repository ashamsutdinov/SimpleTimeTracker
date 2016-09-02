using System;
using System.Linq;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.TimeRecords;

namespace TimeTracker.Service.Base.Validation.TimeRecords
{
    internal class TimeRecordListAccessRightsValidationRule :
        RequestValidationRule<TimeRecordsFilterData>
    {
        private readonly string[] _requiredRoles;

        public TimeRecordListAccessRightsValidationRule(params string[] requiredRoles)
        {
            _requiredRoles = requiredRoles;
        }

        protected override void EvaluateInternal(IUser user, IUserSession userSession, Request<TimeRecordsFilterData> request)
        {
            if (request.Data.LoadAllUsers && !user.Roles.Select(r => r.Id).Intersect(_requiredRoles).Any())
            {
                throw new UnauthorizedAccessException("User does not have enough rights to request time record list");
            }
        }
    }
}