namespace TimeTracker.Contract.IoC
{
    public interface IServiceResolver
    {
        TInterface Resolve<TInterface>();
    }
}
