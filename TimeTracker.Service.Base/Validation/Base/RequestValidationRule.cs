using System;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Base.Validation.Base
{
    internal abstract class RequestValidationRule<TData> :
        ValidationRule
    {
        private readonly bool _verifyRequestType;

        protected RequestValidationRule(bool verifyRequestType)
        {
            _verifyRequestType = verifyRequestType;
        } 

        public override void Evaluate(IUser user, IUserSession userSession, Request request)
        {
            var specificRequest = request as Request<TData>;
            if (_verifyRequestType && specificRequest == null)
            {
                throw new InvalidOperationException("Invalid request");
            }
            EvaluateInternal(user, userSession, specificRequest);
        }

        protected abstract void EvaluateInternal(IUser user, IUserSession userSession, Request<TData> request);
    }
}