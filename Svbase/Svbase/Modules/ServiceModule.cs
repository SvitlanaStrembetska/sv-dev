using Autofac;
using Svbase.Service.Factory;

namespace Svbase.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(ServiceManager)).As(typeof(IServiceManager)).InstancePerLifetimeScope();
        }
    }
}