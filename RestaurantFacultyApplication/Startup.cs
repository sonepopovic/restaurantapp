using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestaurantFacultyApplication.Startup))]
namespace RestaurantFacultyApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
