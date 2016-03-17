using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HomeworkWeek1.Startup))]
namespace HomeworkWeek1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
