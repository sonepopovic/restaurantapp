using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using RestaurantFacultyApplication.Models;
using RestaurantFacultyApplication.Repositories;
namespace RestaurantFacultyApplication.Persistence
{
    public class TableRepository : Repository<Table>, ITableRepository
    {
        public TableRepository(RestaurantModelContext context) : base(context)
        {
        }
        public RestaurantModelContext RestaurantModelContext
        {
            get { return Context as RestaurantModelContext; }
        }

        public Dictionary<int,Table> GetAllFreeTablesForRestaurant(ReservationParameters resParameters)
        {
            IEnumerable<Table> tablesForRes = GetAllTablesForRestaurant(resParameters.Id);
            var result = from reservation in RestaurantModelContext.Reservations
                join restable in RestaurantModelContext.Resevated_tables
                on reservation.RE_ID equals restable.RE_ID
                where
                reservation.ID.Equals(resParameters.Id) &&
                (reservation.RE_DATE < DbFunctions.AddHours(resParameters.ReservationTime, resParameters.Duration))&&
                (DbFunctions.AddHours(reservation.RE_DATE, reservation.RE_LENGTH) > resParameters.ReservationTime)
                select new {restable.ID};

            Dictionary<int, Table> tablesForResponse = tablesForRes.ToDictionary(table => table.ID, table => table);
            foreach (var element in result.ToList())
            {
                if(tablesForResponse.ContainsKey(element.ID))
                    tablesForResponse.Remove(element.ID);
            }
            
            return tablesForResponse; 
        }

        public IEnumerable<Table> GetAllTablesForRestaurant(int resId)
        {
            return RestaurantModelContext.Tables.Where(u => u.RES_ID==resId);
        }

        public Table GetTableByRowAndColumn(int resId,int row, int column)
        {
            List<Table> listOfRestaurantTables = GetAllTablesForRestaurant(resId).ToList();
            return listOfRestaurantTables.SingleOrDefault(x => x.ROW == row && x.COLUMN == column);
        }

    }
}