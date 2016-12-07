﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantFacultyApplication.Repositories;
namespace RestaurantFacultyApplication.Persistence
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(RestaurantModelContext context) : base(context)
        {
            
        }
        public RestaurantModelContext RestaurantModelContext
        {
            get { return Context as RestaurantModelContext; }
        }


    }
}