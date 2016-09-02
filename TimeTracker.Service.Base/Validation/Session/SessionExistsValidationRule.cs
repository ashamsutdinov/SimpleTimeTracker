using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Validation.Session
{
    internal class SessionExistsValidationRule : 
        ValidationRule
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