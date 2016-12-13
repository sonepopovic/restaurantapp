using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RestaurantFacultyApplication.Models;

namespace RestaurantFacultyApplication.Controllers
{
    [Authorize]
    public class WelcomeController : Controller
    {
        public Boolean IsManagerOfSystem()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("ManagerOfSystem"))
                {
                    return true;
                }
                return false;
                
            }
            return false;
        }
        // GET: Welcome
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                //	ApplicationDbContext context = new ApplicationDbContext();
                //	var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                //var s=	UserManager.GetRoles(user.GetUserId());
                ViewBag.displayMenu = "No";

                if (IsManagerOfSystem())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return View();
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }


            return View();
        }
    }
}