using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeacherWebRole.Startup))]
namespace TeacherWebRole
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
