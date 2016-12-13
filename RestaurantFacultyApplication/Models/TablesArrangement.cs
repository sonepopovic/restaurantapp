using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantFacultyApplication.Models
{
    public class TablesArrangement
    {
        public Dictionary<int, Dictionary<int, Table>> Arrangement { get; set; }

        public ReservationParameters ReservationParameters { get; set; }

    }
}