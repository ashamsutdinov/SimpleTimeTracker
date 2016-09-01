using System;
using System.Security.Authentication;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Data.Entities;
using TimeTracker.Service.Contract.Data;

namespace TimeTracker.Service.Base.Security
{
    internal class UserByLoginExistsPolicy :
        AnonymousRequestSecurityPolicy<LoginData>
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