using System;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal class SessionExpicationPolicy :
        SecurityPolicy
    {
        private readonly IUserDataProvider _userDataProvider;

        public SessionExpicationPolicy(IUserDataProvider userDataProvider)
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