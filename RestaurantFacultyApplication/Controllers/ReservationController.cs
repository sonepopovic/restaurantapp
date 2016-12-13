using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurantFacultyApplication.Controllers
{
    public class ReservationController : Controller
    {
        // GET: Reservation
        public ActionResult ShowReservation()
        {
            return View();
        }
    }
}