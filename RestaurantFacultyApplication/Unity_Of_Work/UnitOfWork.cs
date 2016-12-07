using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using RestaurantFacultyApplication.Repositories;
using RestaurantFacultyApplication.Persistence;
namespace RestaurantFacultyApplication.Unity_Of_Work
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestaurantModelContext _context;
       
        public ICustomerRepository Customers { get; private set; }
        public IFriendRepository Friends { get; private set; }
        public IFriendRatingRepository FriendsRating { get; private set; }
        public IInvitationRepository Invitations { get; private set; }
        public IMealRepository Meals { get; private set; }
        public IReservationRepository Reservations { get; private set; }
        public IResevatedTableRepository ResevatedTables { get; private set; }
        public IRestaurantRepository Restaurants { get; private set; }
        public ITableRepository Tables { get; private set; }
        public IUserRepository Users { get; private set; }

        public UnitOfWork(RestaurantModelContext context)
        {
            _context = context;
            Customers = new CustomerRepository(_context);
            Friends = new FriendRepository(_context);
            FriendsRating = new FriendRatingRepository(_context);
            Invitations = new InvitationRepository(_context);
            Meals = new MealRepository(_context);
            Reservations = new ReservationRepository(_context);
            ResevatedTables = new ResevatedTableRepository(_context);
            Restaurants = new RestaurantRepository(_context);
            Tables = new TableRepository(_context);
            Users = new UserRepository(_context);
        }
        public int Complete()
        {
            
                return _context.SaveChanges();
            
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}