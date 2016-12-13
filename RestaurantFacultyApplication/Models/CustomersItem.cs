using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantFacultyApplication.Models
{
    public class CustomersItem
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }

        public bool Friend { get; set; }
    }
}