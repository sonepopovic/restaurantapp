using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantFacultyApplication.Repositories;

namespace RestaurantFacultyApplication.Persistence
{
    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(RestaurantModelContext context) : base(context)
        {
        }
        public RestaurantModelContext RestaurantModelContext
        {
            get { return Context as RestaurantModelContext; }
        }

        
    }
}