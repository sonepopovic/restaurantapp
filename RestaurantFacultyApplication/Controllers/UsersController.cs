using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using RestaurantFacultyApplication;
using RestaurantFacultyApplication.Models;
using RestaurantFacultyApplication.Unity_Of_Work;

namespace RestaurantFacultyApplication.Controllers
{
    [Authorize]
    public class UsersController : Controller
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
       
        // GET: Users
        public ActionResult Index()
        {
            using (var unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                IEnumerable<User> model = unitOfWork.Users.GetAllManagers();
                return View(model.ToList());
            }
            
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                User user = unitOfWork.Users.Get((int)id);

                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            
            
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                
                ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("ManagerOfSystem") && !u.Name.Contains("Guest"))
                                             .ToList(), "Name", "Name");
                return View();
            }
            
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EMAIL,PASSWORD,ROLE,ACTIVATED")] User user)
        {
            if (ModelState.IsValid)
            {
                
                var newUser = new ApplicationUser { UserName = user.EMAIL, Email = user.EMAIL};
                var result = await UserManager.CreateAsync(newUser, user.PASSWORD);

                if (result.Succeeded)
                {
                    using (UnitOfWork unityOfWork = new UnitOfWork(new RestaurantModelContext()))
                    {
                        RestaurantFacultyApplication.User userDatabase = new User();
                        userDatabase.EMAIL = user.EMAIL;
                        userDatabase.ROLE = user.ROLE;
                        userDatabase.RES_ID = user.RES_ID;
                        userDatabase.ACTIVATED = user.ACTIVATED ?? false;
                        var userIdentity = await UserManager.FindByEmailAsync(user.EMAIL);
                        userIdentity.EmailConfirmed = user.ACTIVATED ?? false;
                        userDatabase.PASSWORD = "";
                        unityOfWork.Users.Add(userDatabase);
                        unityOfWork.Complete();
                    }
                    
                    
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(newUser.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(newUser.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    await this.UserManager.AddToRoleAsync(newUser.Id, user.ROLE);
                    return RedirectToAction("Index");
                }


                return View(user);
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
             User user = unitOfWork.Users.Get((int)id);
             if (user == null)
             {
                    return HttpNotFound();
             }
                return View(user);
            }
            
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,EMAIL,PASSWORD,ROLE,ACTIVATED")] User user)
        {
            if (ModelState.IsValid)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
                {
                    User currentUser = unitOfWork.Users.Get(user.ID);
                    if (currentUser == null)
                        return HttpNotFound();
                    var userIdentity = await UserManager.FindByEmailAsync(currentUser.EMAIL);
                    if (user.ACTIVATED != null)
                    {
                        userIdentity.EmailConfirmed = user.ACTIVATED.Value;
                        
                        await UserManager.UpdateAsync(userIdentity);
                    }
                    currentUser.ACTIVATED = user.ACTIVATED;
                    unitOfWork.Users.Update(currentUser);
                    unitOfWork.Complete();
                    return RedirectToAction("Index");
                }
                
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                User user = unitOfWork.Users.Get((int) id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
           
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                                 
                User user = unitOfWork.Users.Get(id);

                var userIdentity = await UserManager.FindByEmailAsync(user.EMAIL);
                var logins = userIdentity.Logins;
                var rolesForUser = await UserManager.GetRolesAsync(userIdentity.Id);
                using (var transaction = new ApplicationDbContext().Database.BeginTransaction())
                {
                    foreach (var login in logins.ToList())
                    {
                        await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            // item should be the name of the role
                            var result = await UserManager.RemoveFromRoleAsync(userIdentity.Id, item);
                        }
                    }

                    await UserManager.DeleteAsync(userIdentity);
                    transaction.Commit();

                }
                unitOfWork.Users.Remove(user);
                unitOfWork.Complete();

                return RedirectToAction("Index");
            }
            
        }
        //GET: Users/ManagerToRest/5 
        public ActionResult ManagerToRest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                User user = unitOfWork.Users.Get((int)id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                ManagerRestaurantViewModel mrestv = new ManagerRestaurantViewModel();
                mrestv.Email = user.EMAIL;
                //SelectList restaurants = new SelectList(unitOfWork.Restaurants.GetAllUnmanagedRestaurants(),"ID","NAME","");
                var restaurants = unitOfWork.Restaurants.GetAll().Select(x =>
                    new SelectListItem
                    {
                        Value = x.ID.ToString(),
                        Text = x.NAME
                    });

                mrestv.Restaurants= new SelectList(restaurants, "Value", "Text");

                

                return View(mrestv);
            }
        }

        //POST:
        [HttpPost, ActionName("ManagerToRest")]
        [ValidateAntiForgeryToken]
        public ActionResult ManagerToRestConfirmed(ManagerRestaurantViewModel model)
        {

            if (ModelState.IsValid)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
                {
                    Restaurant rest = unitOfWork.Restaurants.Get(model.ResId);
                    User user = unitOfWork.Users.SingleOrDefault(a => a.EMAIL == model.Email);
                    user.RES_ID = rest.ID;
                    unitOfWork.Users.Update(user);
                    unitOfWork.Complete();
                }
                return RedirectToAction("Index");
            }

            return View(model);

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               
            }
            base.Dispose(disposing);
        }
    }
}
