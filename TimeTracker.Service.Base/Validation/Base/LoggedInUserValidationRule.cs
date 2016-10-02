using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Base.Validation.Base
{
    internal abstract class LoggedInUserValidationRule :
        ValidationRule
    {
        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            if (user == null)
            {
                throw new UnauthorizedAccessException("User is not logged in");
            }
        }

        protected abstract void EvaluateInternal(IUser user, IUserSession userSession, Request request);
    }
}