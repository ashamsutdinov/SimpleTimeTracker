using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal class UserExistsPolicy : 
        SecurityPolicy
    {
        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            if (user == null)
            {
                throw new UnauthorizedAccessException("User was not found");
            }
        }
    }
}