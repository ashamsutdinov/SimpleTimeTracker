using System;
using System.ServiceModel;
using TimeTracker.Contract;
using TimeTracker.RestService.Utils;

namespace TimeTracker.RestService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TimeTrackingService : 
        ITimeTrackingService
    {
        private readonly NonceGenerator _nonceGenerator = new NonceGenerator();

        public Response<string> GetNonce(Request request)
        {
            return SafeInvoke(() => _nonceGenerator.GetNonce(request.ClientId));
        }

        private Response<TData> SafeInvoke<TData>(Func<TData> action)
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
