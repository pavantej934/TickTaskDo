using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TickTaskDoe.Startup))]
namespace TickTaskDoe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
