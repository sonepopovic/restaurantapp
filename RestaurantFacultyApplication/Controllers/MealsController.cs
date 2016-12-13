using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using RestaurantFacultyApplication;
using RestaurantFacultyApplication.Unity_Of_Work;

namespace RestaurantFacultyApplication.Controllers
{
    [Authorize(Roles="Manager")]
    public class MealsController : Controller
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
        // GET: Meals
        public ActionResult Index()
        {
            
            
                using (var unitOfWork = new UnitOfWork(new RestaurantModelContext()))
                {
                    var userIdentity = UserManager.FindById(User.Identity.GetUserId());
                    User user = unitOfWork.Users.FindUserByEmail(userIdentity.Email);
                    Restaurant restaurant = unitOfWork.Restaurants.Get((int) user.RES_ID);
                    ViewBag.Title = restaurant.NAME;
                    return View(unitOfWork.Meals.GetAllMealsForRestaurant((int)user.RES_ID));
                }
            
            
        }

        // GET: Meals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                Meal meal = unitOfWork.Meals.Get((int)id);
                if (meal == null)
                {
                    return HttpNotFound();
                }
                Restaurant restaurant = unitOfWork.Restaurants.Get(meal.RES_ID);
                ViewBag.Title = restaurant.NAME;
                return View(meal);
            }
            
        }

        // GET: Meals/Create
        public ActionResult Create()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                var userIdentity = UserManager.FindById(User.Identity.GetUserId());
                User user = unitOfWork.Users.FindUserByEmail(userIdentity.Email);
                Restaurant restaurant = unitOfWork.Restaurants.Get((int)user.RES_ID);
                ViewBag.Title = restaurant.NAME;

                return View();

            }
        }

        // POST: Meals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RES_ID,NAME,DECRIPTION,PRICE")] Meal meal)
        {
            if (ModelState.IsValid)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
                {
                    var userIdentity = UserManager.FindById(User.Identity.GetUserId());
                    User user = unitOfWork.Users.FindUserByEmail(userIdentity.Email);
                    meal.RES_ID = (int)user.RES_ID;
                    unitOfWork.Meals.Add(meal);
                    unitOfWork.Complete();
                    return RedirectToAction("Index");
                }   
            }
            return View(meal);
        }

        // GET: Meals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                Meal meal = unitOfWork.Meals.Get((int)id);
                if (meal == null)
                {
                    return HttpNotFound();
                }
                Restaurant restaurant = unitOfWork.Restaurants.Get(meal.RES_ID);
                ViewBag.Title = restaurant.NAME;
                
                return View(meal);

            }
            
        }

        // POST: Meals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RES_ID,NAME,DECRIPTION,PRICE")] Meal meal)
        {
            if (ModelState.IsValid)
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
                {
                    Meal mealFromDatab = unitOfWork.Meals.Get(meal.ID);
                    mealFromDatab.DECRIPTION = meal.DECRIPTION;
                    mealFromDatab.NAME = meal.NAME;
                    mealFromDatab.PRICE = meal.PRICE;
                    unitOfWork.Meals.Update(mealFromDatab);
                    unitOfWork.Complete();
                }
                return RedirectToAction("Index");
            }
            return View(meal);
        }

        // GET: Meals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                Meal meal = unitOfWork.Meals.Get((int) id);
                if (meal == null)
                {
                    return HttpNotFound();
                }
                Restaurant restaurant = unitOfWork.Restaurants.Get((int)meal.RES_ID);
                ViewBag.Title = restaurant.NAME;
                
                return View(meal);
            }
            
            
        }

        // POST: Meals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                Meal meal = unitOfWork.Meals.Get(id);
                unitOfWork.Meals.Remove(meal);
                unitOfWork.Complete();
            }
            return RedirectToAction("Index");
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
