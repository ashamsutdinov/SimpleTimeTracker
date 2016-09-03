using System;
using System.Linq;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.UserManagement;

namespace TimeTracker.Service.Base.Validation.UserManagement
{
    internal class UserSettingsEditRightsValidationRule :
        RequestValidationRule<UserSettingItemList>
    {
        private readonly string[] _requiredRoles;

        public UserSettingsEditRightsValidationRule(params string[] requiredRoles) : 
            base(true)
        {
            _requiredRoles = requiredRoles;
        }

        protected override void EvaluateInternal(IUser user, IUserSession userSession, Request<UserSettingItemList> request)
        {
            if (request.Data.UserId != 0 && user.Id != request.Data.UserId && !user.Roles.Select(r=>r.Id).Intersect(_requiredRoles).Any())
            {
                throw new UnauthorizedAccessException("User does not have enough rights to perform manipulations with users settings");   
            }
        }
    }
}
