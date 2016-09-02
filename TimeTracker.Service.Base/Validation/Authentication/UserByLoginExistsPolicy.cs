using System.Security.Authentication;
using TimeTracker.Contract.Data;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Validation.Authentication
{
    internal class UserByLoginExistsPolicy :
        AnonymousRequestValidationRule<LoginData>
    {
        private readonly IUserDataProvider _userDataProvider;

        public UserByLoginExistsPolicy(IUserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
        }

        protected override void EvaluateAnonymous(Request<LoginData> request)
        {
            var login = request.Data.Login;
            var user = _userDataProvider.GetUser(login);
            if (user == null)
            {
                throw new AuthenticationException("Invalid login or password");
            }
        }
    }
}