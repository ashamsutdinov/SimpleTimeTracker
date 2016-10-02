using System;
using System.Linq;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Base.Validation.UserManagement
{
    internal class UserManagementPermissionsValidationRule :
        LoggedInUserValidationRule
    {
        private readonly string[] _requiredRoles;

        public UserManagementPermissionsValidationRule(params string[] requiredRoles)
        {
            _requiredRoles = requiredRoles;
        }

        protected override void EvaluateInternal(IUser user, IUserSession userSession, Request request)
        {
            var hasRequiredRoles = user.Roles.Select(r => r.Id).Intersect(_requiredRoles).Any();
            if (!hasRequiredRoles)
            {
                throw new UnauthorizedAccessException("User does not have enough rights for user management");
            }
        }
    }
}