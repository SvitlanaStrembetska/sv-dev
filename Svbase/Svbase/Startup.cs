using Microsoft.Owin;
using Owin;
using Svbase.Core.Data;
using Svbase.Core.Migrations.DbInitializer;

[assembly: OwinStartupAttribute(typeof(Svbase.Startup))]
namespace Svbase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            new CreateIfNotExistWithSeed(new ApplicationDbContext()).InitializeDb();
        }
    }
}
