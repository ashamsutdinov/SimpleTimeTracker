using System;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Base.Validation.Session
{
    internal class SessionIsNotExpiredValidationRule :
        ValidationRule
    {
        private readonly IUserDataProvider _userDataProvider;

        public SessionIsNotExpiredValidationRule(IUserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
        }

        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            if (userSession == null || userSession.Expired || userSession.DateTime.AddSeconds(userSession.Expiration) < DateTime.UtcNow)
            {
                if (userSession != null)
                {
                    userSession.Expired = true;
                    _userDataProvider.SaveSession(userSession);
                }
                throw new InvalidOperationException("Session was expired");
            }
        }
    }
}