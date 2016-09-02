using System.Security.Authentication;
using TimeTracker.Contract.Data;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Validation.Authentication
{
    internal class UserIsActiveValudationRule :
        AnonymousRequestValidationRule<LoginData>
    {
        private readonly IUserDataProvider _userDataProvider;

        public UserIsActiveValudationRule(IUserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
        }

        protected override void EvaluateAnonymous(Request<LoginData> request)
        {
            var login = request.Data.Login;
            var user = _userDataProvider.GetUser(login);
            if (user.StateId != "active")
            {
                throw new AuthenticationException("User was disabled or deleted");
            }
        }
    }
}