using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using TimeTracker.Contract.IoC;

namespace TimeTracker.RestService.IoC
{
    public class UnityServiceResolver :
        IServiceResolver
    {
        private static readonly UnityContainer Container;

        static UnityServiceResolver()
        {
            Container = new UnityContainer();
            Container.LoadConfiguration();
        }

        public TInterface Resolve<TInterface>()
        {
            return Container.Resolve<TInterface>();
        }
    }
}