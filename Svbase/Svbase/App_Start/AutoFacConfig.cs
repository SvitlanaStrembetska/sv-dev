using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Svbase.Modules;

namespace Svbase
{
    public class AutoFacConfig
    {
        public static void RegisterModules()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();

            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}