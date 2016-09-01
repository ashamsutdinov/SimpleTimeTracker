using System.Security.Authentication;
using TimeTracker.Contract.Data;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal class UserActiveStatePolicy :
        AnonymousRequestSecurityPolicy<LoginData>
    {
        private readonly IUserDataProvider _userDataProvider;

        public UserActiveStatePolicy(IUserDataProvider userDataProvider)
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