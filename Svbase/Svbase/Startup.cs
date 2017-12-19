using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Svbase.Startup))]
namespace Svbase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
