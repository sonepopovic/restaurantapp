﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantFacultyApplication.Repositories
{
    public interface IMealRepository : IRepository<Meal>
    {
        IEnumerable<Meal> GetAllMealsForRestaurant(int resId);
    }
}
