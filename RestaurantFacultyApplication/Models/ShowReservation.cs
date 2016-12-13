using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantFacultyApplication.Models
{
    public class ShowReservation
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Reservation time")]
        public System.DateTime ReservationTime { get; set; }
        [Display(Name = "Duration")]
        public short Duration { get; set; }
    }
}