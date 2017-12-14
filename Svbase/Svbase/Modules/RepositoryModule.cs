using Autofac;
using Svbase.Core.Repositories.Factory;

namespace Svbase.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(RepositoryManager)).As(typeof(IRepositoryManager)).SingleInstance().PreserveExistingDefaults();
        }
    }
}