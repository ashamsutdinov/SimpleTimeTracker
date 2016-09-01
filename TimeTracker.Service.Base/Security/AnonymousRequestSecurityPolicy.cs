using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal abstract class AnonymousRequestSecurityPolicy :
        SecurityPolicy
    {
        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            EvaluateAnonymous(request);
        }

        protected abstract void EvaluateAnonymous(Request request);
    }

    internal abstract class AnonymousRequestSecurityPolicy<TData> :
        SpecificRequestSecurityPolicy<TData>
    {
        protected override void EvaluateInternal(IUser user, IUserSession userSession, Request<TData> request)
        {
            EvaluateAnonymous(request);
        }

        protected abstract void EvaluateAnonymous(Request<TData> request);
    }
}