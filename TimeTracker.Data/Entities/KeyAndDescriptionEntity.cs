namespace TimeTracker.Data.Entities
{
    public class KeyAndDescriptionEntity :
        Entity<string>
    {
        public string Description { get; set; }
    }
}