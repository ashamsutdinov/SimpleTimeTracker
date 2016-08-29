namespace TimeTracker.Contract.Data.Entities.Base
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}