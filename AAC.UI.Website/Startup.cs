using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AAC.UI.Website.Startup))]
namespace AAC.UI.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
