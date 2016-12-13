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
    
    public class TablesController : Controller
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

        //GET Tables
        public ActionResult TablesArrangement()
        {
           
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                TablesArrangement tablesArrangement = new TablesArrangement();
                var userIdentity = UserManager.FindById(User.Identity.GetUserId());
                var user = unitOfWork.Users.FindUserByEmail(userIdentity.Email);
                List<Table> listOfTables = unitOfWork.Tables.GetAllTablesForRestaurant((int)user.RES_ID).ToList();
                tablesArrangement.Arrangement = new Dictionary<int, Dictionary<int, Table>>();
                foreach (var item in listOfTables)
                {
                    if(!tablesArrangement.Arrangement.ContainsKey(item.ROW))
                        tablesArrangement.Arrangement.Add(item.ROW,new Dictionary<int, Table>());
                    tablesArrangement.Arrangement[item.ROW][item.COLUMN] = item;
                }
                ViewBag.Title = unitOfWork.Restaurants.Get((int) user.RES_ID).NAME;
                return View(tablesArrangement);
            }
            
        }
        // POST: Tables
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TablesArangementConfirmed(string action, int i, int j)
        {
            if (action.Equals("add"))
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
                {
                    var userIdentity = UserManager.FindById(User.Identity.GetUserId());
                    var user = unitOfWork.Users.FindUserByEmail(userIdentity.Email);
                    Table newTable = new Table();
                    newTable.ROW = i;
                    newTable.COLUMN = j;
                    newTable.RES_ID = (int)user.RES_ID;
                    unitOfWork.Tables.Add(newTable);
                    unitOfWork.Complete();
                    return RedirectToAction("TablesArrangement");
                    
                }
                
            }

            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                var userIdentity = UserManager.FindById(User.Identity.GetUserId());
                var user = unitOfWork.Users.FindUserByEmail(userIdentity.Email);
                Table removeTable = unitOfWork.Tables.GetTableByRowAndColumn((int)user.RES_ID, i, j);
                unitOfWork.Tables.Remove(removeTable);
                unitOfWork.Complete();
                return RedirectToAction("TablesArrangement");

            }
            
            
            
        }
        // POST: Tables
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowFreeTables(ReservationParameters model)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
            {
                TablesArrangement tablesArrangement = new TablesArrangement();
                tablesArrangement.ReservationParameters = model;
                Dictionary<int, Table> freeTables = unitOfWork.Tables.GetAllFreeTablesForRestaurant(model);
                tablesArrangement.Arrangement = new Dictionary<int, Dictionary<int, Table>>();
                foreach (var item in freeTables)
                {
                    if (!tablesArrangement.Arrangement.ContainsKey(item.Value.ROW))
                        tablesArrangement.Arrangement.Add(item.Value.ROW, new Dictionary<int, Table>());
                    tablesArrangement.Arrangement[item.Value.ROW][item.Value.COLUMN] = item.Value;
                    
                    
                }
                ViewBag.Title = model.Name;
                return View(tablesArrangement);
            }
           
        }

        // POST: Tables
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TableReservationConfirmed(string action, int i, int j,int table,DateTime restime,short duration,int resid)
        {
            if (action.Equals("reservate"))
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(new RestaurantModelContext()))
                {
                    Reservation newReservation = new Reservation();
                    var userIdentity = UserManager.FindById(User.Identity.GetUserId());
                    var userDatabase = unitOfWork.Users.FindUserByEmail(userIdentity.Email);
                    newReservation.CUS_ID = userDatabase.ID;
                    newReservation.RE_DATE = restime; 
                    newReservation.RE_LENGTH = duration;
                    newReservation.ID = resid;                
                    unitOfWork.Reservations.Add(newReservation);
                    unitOfWork.Complete();

                    Resevated_table resevatedTable = new Resevated_table();
                    resevatedTable.ID = table;                       
                    resevatedTable.RE_ID = newReservation.RE_ID;           
                    unitOfWork.ResevatedTables.Add(resevatedTable);
                    unitOfWork.Complete();
                    ShowReservation showReservation = new ShowReservation();
                    showReservation.Duration = (short) newReservation.RE_LENGTH;
                    showReservation.Name = userDatabase.EMAIL;
                    showReservation.ReservationTime = (DateTime)newReservation.RE_DATE;
                    ViewBag.Title = unitOfWork.Restaurants.Get(newReservation.ID).NAME;
                    return View(showReservation);
                }
            }
            return View();
        }
        
    }
}