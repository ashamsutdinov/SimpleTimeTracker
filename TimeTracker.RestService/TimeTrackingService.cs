using System;
using System.ServiceModel;
using TimeTracker.Contract;
using TimeTracker.Contract.Requests;
using TimeTracker.Contract.Requests.Base;
using TimeTracker.Contract.Responses;
using TimeTracker.Contract.Responses.Base;
using TimeTracker.RestService.Utils;

namespace TimeTracker.RestService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TimeTrackingService : 
        ITimeTrackingService
    {
        private readonly AuthenticationHelper _authenticationHelper = new AuthenticationHelper();

        public Response<string> GetNonce(Request request)
        {
            return SafeInvoke(() => _authenticationHelper.GetNonce(request.ClientId));
        }

        public Response<LoginResponse> Login(LoginRequest request)
        {
            return SafeInvoke(() =>
            {
                var login = request.Data.Login;
                var passwordComponents = _authenticationHelper.Xor(request.Data.Password).Split('#');
                var password = passwordComponents[0];
                var clientId = passwordComponents[1];
                var nonce = passwordComponents[2];

                return new LoginResponse();
            });
        }

        private static Response<TData> SafeInvoke<TData>(Func<TData> action)
        {
            try
            {
                var data = action();
                return Response<TData>.Success(data);
            }
            catch (Exception ex)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = color;
                return Response<TData>.Fail(ex.Message);
            }
        }
    }
}
