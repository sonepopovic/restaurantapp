using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantFacultyApplication.Repositories;
namespace RestaurantFacultyApplication.Persistence
{
    public class MealRepository : Repository <Meal>, IMealRepository
    {
        public MealRepository(RestaurantModelContext context) : base(context)
        {
        }
        public RestaurantModelContext RestaurantModelContext
        {
            get { return Context as RestaurantModelContext; }
        }

        public IEnumerable<Meal> GetAllMealsForRestaurant(int resId)
        {
            
            return RestaurantModelContext.Meals.ToList().Where(a => a.RES_ID == resId);
                           
        }
    }
}