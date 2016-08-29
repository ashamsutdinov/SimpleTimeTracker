using System;
using System.Security.Authentication;
using TimeTracker.Contract.Data;
using TimeTracker.Contract.Requests;
using TimeTracker.Contract.Responses;
using TimeTracker.Contract.Utils;

namespace TimeTracker.Contract
{
    public abstract class TimeTrackerServiceBase : 
        ITimeTrackerService
    {
        private readonly CryptographyHelper _cryptographyHelper = new CryptographyHelper();

        protected readonly IUserDataProvider UserDataProvider;

        protected readonly ITimeRecordDataProvider TimeTrackingDataProvider;

        protected TimeTrackerServiceBase(IUserDataProvider userDataProvider, ITimeRecordDataProvider timeRecordDataProvider)
        {
            UserDataProvider = userDataProvider;
            TimeTrackingDataProvider = timeRecordDataProvider;
        }

        public Response<long> Heartbit(Request<long> request)
        {
            return SafeInvoke(r => DateTime.UtcNow.Ticks, request);
        }

        public virtual Response<string> GetNonce(Request request)
        {
            return SafeInvoke(r => _cryptographyHelper.GetNonce(request.ClientId), request);
        }

        public virtual Response<TicketData> Login(Request<LoginData> request)
        {
            return SafeInvoke(r =>
            {
                var login = request.Data.Login;
                DecryptedPasswordData paswordData;
                var validPasswordSyntax = _cryptographyHelper.VerifyPasswordSyntax(request.Data.Password, out paswordData);
                if (!validPasswordSyntax)
                {
                    throw new AuthenticationException("Invalid authentication nonce");
                }

                //lookup user
                //throw new AuthenticationException("User not found");

                //check password
                //throw new AuthenticationException("Invalid login or password");

                //check user status
                //throw new AuthenticationException("User was disabled or deleted");

                //create session
                var sessionId = 0;
                var userId = 0;

                //create ticket
                var ticket = _cryptographyHelper.GetSessionTicket(sessionId, userId, paswordData.ClientId);

                return new TicketData
                {
                    Ticket = ticket
                };
            }, request);
        }

        private Response<TData> SafeInvoke<TData>(Func<Request, TData> action, Request request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.Ticket))
                {
                    DecryptedTicketData ticketData;
                    if (!_cryptographyHelper.VerifyTicketSyntax(request.Ticket, out ticketData))
                    {
                        return Response<TData>.NotAcceptable("Invalid ticket");
                    }

                    //check if session-to-user binding is correct
                    //return Response<TData>.NotAcceptable("Invalid ticket");

                    //check if ticket stored in db is correct
                    //return Response<TData>.NotAcceptable("Invalid ticket");
                }

                var data = action(request);
                return Response<TData>.Success(data);
            }
            catch (UnauthorizedAccessException uaex)
            {
                return TraceUnauthorizedException<TData>(uaex);
            }
            catch (AuthenticationException aex)
            {
                return TraceAuthenticationException<TData>(aex);
            }
            catch (Exception ex)
            {
                return TraceException<TData>(ex);
            }
        }

        private static Response<TData> TraceException<TData>(Exception ex)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.ToString());
            Console.ForegroundColor = color;
            return Response<TData>.Fail(ex.Message);
        }

        private static Response<TData> TraceUnauthorizedException<TData>(UnauthorizedAccessException uaex)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(uaex.ToString());
            Console.ForegroundColor = color;
            return Response<TData>.Forbidden(uaex.Message);
        }

        private static Response<TData> TraceAuthenticationException<TData>(AuthenticationException aex)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(aex.ToString());
            Console.ForegroundColor = color;
            return Response<TData>.Unauthorized(aex.Message);
        }
    }
}