namespace TimeTracker.Contract.Data.Entities
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}