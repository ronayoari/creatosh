using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentWebRole.Startup))]
namespace StudentWebRole
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
