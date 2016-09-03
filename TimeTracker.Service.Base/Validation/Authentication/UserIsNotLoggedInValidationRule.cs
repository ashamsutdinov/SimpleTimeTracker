using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Base.Validation.Authentication
{
    internal class UserIsNotLoggedInValidationRule :
        ValidationRule
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