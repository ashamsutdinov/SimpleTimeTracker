using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace TimeTracker.RestService.Utils
{
    public static class UnityHelper
    {
        private static readonly UnityContainer Container;

        static UnityHelper()
        {
            Container = new UnityContainer();
            Container.LoadConfiguration();
        }
        
        public static TInterface Resolve<TInterface>()
        {
            return Container.Resolve<TInterface>();
        }
    }
}