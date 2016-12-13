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

        public IEnumerable<friend> GetAllFriendsForUser(int userId)
        {
            return RestaurantModelContext.friends.Where(x => x.ID == userId);
        }

        public void RemoveFromFriends(int userId, int friendId)
        {
            friend elementForDelete = RestaurantModelContext.friends.Find(userId, friendId);
            RestaurantModelContext.friends.Remove(elementForDelete);
        }
    }
}