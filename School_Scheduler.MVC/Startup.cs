using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(School_Scheduler.MVC.Startup))]
namespace School_Scheduler.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
