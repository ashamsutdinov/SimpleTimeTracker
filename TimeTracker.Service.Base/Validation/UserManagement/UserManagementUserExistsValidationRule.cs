using System;
using TimeTracker.Contract.Data;
using TimeTracker.Service.Base.Validation.Base;
using TimeTracker.Service.Contract.Data.Base;
using TimeTracker.Service.Contract.Data.UserManagement;

namespace TimeTracker.Service.Base.Validation.UserManagement
{
    internal class UserManagementUserExistsValidationRule :
        AnonymousRequestValidationRule<UserListItem>
    {
        private readonly IUserDataProvider _userDataProvider;

        public UserManagementUserExistsValidationRule(IUserDataProvider userDataProvider) :
            base(false)
        {
            _userDataProvider = userDataProvider;
        }

        protected override void EvaluateAnonymous(Request<UserListItem> request)
        {
            if (request == null)
                return;

            var user = _userDataProvider.GetUser(request.Data.Id);
            if (user == null)
            {
                throw new InvalidOperationException("User was not found");
            }
        }
    }
}