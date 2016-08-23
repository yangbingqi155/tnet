using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TNet.Startup))]
namespace TNet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
