using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal class SessionExistsPolicy : 
        SecurityPolicy
    {
        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            if (userSession == null)
            {
                throw new InvalidOperationException("Invalid session");
            }
        }
    }
}