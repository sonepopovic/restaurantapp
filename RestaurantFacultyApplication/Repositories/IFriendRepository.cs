using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantFacultyApplication.Repositories
{
    public interface IFriendRepository : IRepository<friend>
    {
        IEnumerable<friend> GetAllFriendsForUser(int userId);
        void RemoveFromFriends(int userId, int friendId);
    }
}
