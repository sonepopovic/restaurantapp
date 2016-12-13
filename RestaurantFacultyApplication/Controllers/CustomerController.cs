using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RestaurantFacultyApplication.Models;
using RestaurantFacultyApplication.Unity_Of_Work;

namespace RestaurantFacultyApplication.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Customer
        public ActionResult Customers()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                var userIdentity = UserManager.FindById(User.Identity.GetUserId());
                var userDatabase = unitOfWork.Users.FindUserByEmail(userIdentity.Email);
                List<friend> friendsList = unitOfWork.Friends.GetAllFriendsForUser(userDatabase.ID).ToList();
                List<CustomersItem> showList = new List<CustomersItem>();
                List<Customer> userList = unitOfWork.Customers.GetAll().ToList();
                Customer user = userList.Find(x => x.ID == userDatabase.ID);
                userList.Remove(user);
                foreach (var item in userList)
                {
                    showList.Add(new CustomersItem{Id = item.ID,Friend = false,Name = item.NAME});
                }
                foreach (var item in showList)
                {
                    foreach (var friend in friendsList)
                    {
                        if (item.Id.Equals(friend.CUS_ID))
                        {
                            item.Friend = true;
                            break;
                        }
                    }
                }
                return View(showList);
            }
            
        }
        // POST: Customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRemoveFriendConfirmed(string action, int id)
        {
            if (action.Equals("add"))
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
                {
                    var userIdentity = UserManager.FindById(User.Identity.GetUserId());
                    var user = unitOfWork.Users.FindUserByEmail(userIdentity.Email);
                    unitOfWork.Friends.Add(new friend{ID = user.ID,CUS_ID = id});
                    unitOfWork.Complete();
                    return RedirectToAction("Customers");
                }
            }
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                var userIdentity = UserManager.FindById(User.Identity.GetUserId());
                var user = unitOfWork.Users.FindUserByEmail(userIdentity.Email);
                unitOfWork.Friends.RemoveFromFriends(user.ID,id);
                unitOfWork.Complete();
                return RedirectToAction("Customers");
            }

        }
    }
}