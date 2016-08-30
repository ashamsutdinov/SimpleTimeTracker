using TimeTracker.Contract.Data.Entities;

namespace TimeTracker.Contract.Data
{
    public interface IUserDataProvider
    {
        IUserSession GetUserSession(int id);

        IUser GetUser(int id);

        IUser GetUser(string login);

        IUserSession CreateNewSession(int userId, string clientId);

        IUserSession SaveSession(IUserSession session);
    }
}