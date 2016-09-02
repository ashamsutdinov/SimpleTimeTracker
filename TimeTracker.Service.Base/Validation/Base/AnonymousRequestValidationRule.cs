using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Validation.Base
{
    internal abstract class AnonymousRequestValidationRule :
        ValidationRule
    {
        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            EvaluateAnonymous(request);
        }

        protected abstract void EvaluateAnonymous(Request request);
    }

    internal abstract class AnonymousRequestValidationRule<TData> :
        RequestValidationRule<TData>
    {
        protected override void EvaluateInternal(IUser user, IUserSession userSession, Request<TData> request)
        {
            EvaluateAnonymous(request);
        }

        protected abstract void EvaluateAnonymous(Request<TData> request);
    }
}