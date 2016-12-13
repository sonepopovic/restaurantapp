using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantFacultyApplication.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetAllManagers();
        User FindUserByEmail(string email);
    }
}
