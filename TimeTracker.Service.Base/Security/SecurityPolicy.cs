using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal abstract class SecurityPolicy
    {
        public abstract void Evaluate(IUser user, IUserSession userSession, Request request);
    }
}
