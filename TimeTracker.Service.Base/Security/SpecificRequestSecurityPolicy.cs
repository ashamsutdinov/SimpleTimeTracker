using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal abstract class SpecificRequestSecurityPolicy<TData> :
        SecurityPolicy
    {
        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            var specificRequest = request as Request<TData>;
            if (specificRequest == null)
            {
                throw new InvalidOperationException("Invalid request");
            }
            EvaluateInternal(user, userSession, specificRequest);
        }

        protected abstract void EvaluateInternal(IUser user, IUserSession userSession, Request<TData> request);
    }
}