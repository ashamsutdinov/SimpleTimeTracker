using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal class SessionOwnershipPolicy :
        SecurityPolicy
    {
        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            var visitorId = user != null ? user.Id : 0;
            var sessionOwnerId = userSession != null ? userSession.UserId : 0;
            if (visitorId == 0 || sessionOwnerId == 0 || visitorId != sessionOwnerId)
            {
                throw new UnauthorizedAccessException("User has no rights to the requested session");
            }
        }
    }
}