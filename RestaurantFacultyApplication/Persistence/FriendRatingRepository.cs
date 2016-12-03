using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantFacultyApplication.Repositories;
namespace RestaurantFacultyApplication.Persistence
{
    public class FriendRatingRepository : Repository<Friend_rating>, IFriendRatingRepository 
    {
        public FriendRatingRepository(RestaurantModelContext context) : base(context)
        {
        }

        public RestaurantModelContext RestaurantModelContext
        {
            get { return Context as RestaurantModelContext; }
        }
    }
}