namespace TimeTracker.Contract.Data.Entities
{
    public interface IKeyAndDescriptionEntity :
        IEntity<string>
    {
        string Description { get; set; }
    }
}