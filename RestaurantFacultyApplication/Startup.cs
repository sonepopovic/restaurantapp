using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using RestaurantFacultyApplication.Models;
using RestaurantFacultyApplication.Unity_Of_Work;

[assembly: OwinStartupAttribute(typeof(RestaurantFacultyApplication.Startup))]
namespace RestaurantFacultyApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        public void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Manager of System role and that would be a administrator of system    
            if (!roleManager.RoleExists("ManagerOfSystem"))
            {

                // Manager of System role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "ManagerOfSystem";
                roleManager.Create(role);

                //Here we create a Manager of System user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "nebojsa";
                user.Email = "soneneso93@gmail.com";

                var userDatabase = new User();
                userDatabase.EMAIL = "soneneso93@gmail.com";
                userDatabase.ACTIVATED = true;
                userDatabase.PASSWORD = "";
                userDatabase.ROLE = "ManagerOfSystem";

                using (var unitOfWork = new UnitOfWork(new RestaurantModelContext()))
                {
                    unitOfWork.Users.Add(userDatabase);
                    unitOfWork.Complete();
                }

                string userPWD = "Passw_001";
                                 
                var chkUser = userManager.Create(user, userPWD);

                //Add default ManagerOfSystem   
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "ManagerOfSystem");

                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Guest role    
            if (!roleManager.RoleExists("Guest"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Guest";
                roleManager.Create(role);

            }
        }
    }
}
