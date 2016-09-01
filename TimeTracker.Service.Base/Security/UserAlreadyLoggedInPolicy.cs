using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal class UserAlreadyLoggedInPolicy :
        SecurityPolicy
    {
        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            if (user != null)
            {
                throw new InvalidOperationException("User already logged in");
            }
        }
    }
}