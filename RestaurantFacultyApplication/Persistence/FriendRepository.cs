using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantFacultyApplication.Repositories;
namespace RestaurantFacultyApplication.Persistence
{
    public class FriendRepository : Repository<friend>, IFriendRepository
    {
        public FriendRepository(RestaurantModelContext context) : base(context)
        {
        }

        public RestaurantModelContext RestaurantModelContext
        {
            get { return Context as RestaurantModelContext; }
        }
    }
}