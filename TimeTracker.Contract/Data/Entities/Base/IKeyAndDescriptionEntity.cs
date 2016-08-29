namespace TimeTracker.Contract.Data.Entities.Base
{
    public interface IKeyAndDescriptionEntity :
        IEntity<string>
    {
        string Description { get; set; }
    }
}