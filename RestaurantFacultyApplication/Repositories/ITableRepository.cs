using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantFacultyApplication.Models;

namespace RestaurantFacultyApplication.Repositories
{
    public interface ITableRepository : IRepository<Table>
    {
        IEnumerable<Table> GetAllTablesForRestaurant(int resId);
        Table GetTableByRowAndColumn(int resId,int row, int column);
        Dictionary<int,Table> GetAllFreeTablesForRestaurant(ReservationParameters resParameters);

    }
}
