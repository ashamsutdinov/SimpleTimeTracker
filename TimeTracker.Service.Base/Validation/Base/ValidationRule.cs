using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;
using TimeTracker.Service.Contract.Data.Base;

namespace TimeTracker.Service.Base.Validation.Base
{
    internal abstract class ValidationRule
    {
        public abstract void Evaluate(IUser user, IUserSession userSession, Request request);
    }
}
