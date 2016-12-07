using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantFacultyApplication;
using RestaurantFacultyApplication.Persistence;
using RestaurantFacultyApplication.Unity_Of_Work;

namespace RestaurantFacultyApplication.Controllers
{
    public class RestaurantsController : Controller
    {
       

        // GET: Restaurants
        public ViewResult Index()
        {
            using (var unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                return View(unitOfWork.Restaurants.GetAll());
            }
            
        }

        // GET: Restaurants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                Restaurant restaurant = unitOfWork.Restaurants.Get((int)id);
                if (restaurant == null)
                {
                    return HttpNotFound();
                }
                return View(restaurant);
            }
            
        }

        // GET: Restaurants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,NAME,DESCRIPTION,RATING_COUNT,RATING_SUM,LATITUDE,LONGITUDE")] Restaurant restaurant)
        public ActionResult Create([Bind(Include = "NAME,DESCRIPTION,LATITUDE,LONGITUDE")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                using (var unitOfWork = new UnitOfWork(new RestaurantModelContext()))
                {
                    unitOfWork.Restaurants.Add(restaurant);
                    unitOfWork.Complete();
                    return RedirectToAction("Index");
                }
                
                
            }

            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            using (var unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                Restaurant restaurant = unitOfWork.Restaurants.Get((int) id);
                if (restaurant == null)
                {
                    return HttpNotFound();
                }
                return View(restaurant);
            }
            
            
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NAME,DESCRIPTION,RATING_COUNT,RATING_SUM,LATITUDE,LONGITUDE")] Restaurant restaurant)
        {

            if (ModelState.IsValid)
            {
                
                using (var unitOfWork = new UnitOfWork(new RestaurantModelContext()))
                {

                    //unitOfWork.Restaurants.Update(restaurant);
                    Restaurant currentRest = unitOfWork.Restaurants.Get(restaurant.ID);
                    if (currentRest == null)
                        return HttpNotFound();

                    currentRest.NAME = restaurant.NAME;
                    currentRest.DESCRIPTION= restaurant.DESCRIPTION;
                    currentRest.LATITUDE = restaurant.LATITUDE;
                    currentRest.LONGITUDE = restaurant.LONGITUDE;
                    unitOfWork.Restaurants.Update(currentRest);
                    unitOfWork.Complete();
                    return RedirectToAction("Index");
                }
                
                
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                Restaurant restaurant = unitOfWork.Restaurants.Get((int) id);
                if (restaurant == null)
                {
                    return HttpNotFound();
                }
                return View(restaurant);
            }
           
            
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                Restaurant restaurant = unitOfWork.Restaurants.Get((int)id);
                unitOfWork.Restaurants.Remove(restaurant);
                unitOfWork.Complete();
                return RedirectToAction("Index");
            }
           
           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
