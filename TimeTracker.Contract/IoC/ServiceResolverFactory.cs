namespace TimeTracker.Contract.IoC
{
    public static class ServiceResolverFactory
    {
        private static IServiceResolver _instance;

        public static void Setup(IServiceResolver serviceResolver)
        {
            _instance = serviceResolver;
        }

        public static IServiceResolver Get()
        {
            return _instance;
        }
    }
}