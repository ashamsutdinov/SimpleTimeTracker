namespace TimeTracker.Contract.Api
{
    public enum SessionState
    {
        Undefined = 0,
        Anonymous = 1,
        LoggedInUser = 2,
        LoggedInManager = 3,
        LoggedInAdministrator = 4
    }
}