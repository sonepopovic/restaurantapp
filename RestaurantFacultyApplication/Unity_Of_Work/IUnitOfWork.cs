using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantFacultyApplication.Repositories;
namespace RestaurantFacultyApplication.Unity_Of_Work
{
    interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IFriendRepository Friends { get; }
        IFriendRatingRepository FriendsRating { get; }
        IInvitationRepository Invitations { get; }
        IMealRepository Meals { get; }
        IReservationRepository Reservations { get; }
        IResevatedTableRepository ResevatedTables { get; }
        IRestaurantRepository Restaurants { get; }
        ITableRepository Tables { get; }
        IUserRepository Users { get; }
        int Complete();
    }
}
