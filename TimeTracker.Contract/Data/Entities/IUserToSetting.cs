namespace TimeTracker.Contract.Data.Entities
{
    public interface IUserToSetting : 
        IUserSetting
    {
        string Value { get; set; }
    }
}