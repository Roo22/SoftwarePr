using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SoftwarePr.Startup))]
namespace SoftwarePr
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
