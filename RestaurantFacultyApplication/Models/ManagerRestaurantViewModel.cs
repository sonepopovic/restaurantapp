using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RestaurantFacultyApplication.Models
{
    public class ManagerRestaurantViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Restaurant")]
        public int ResId { get; set; }
        public IEnumerable<SelectListItem> Restaurants { get; set; }
    }
}