using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantFacultyApplication.Repositories;
namespace RestaurantFacultyApplication.Persistence
{
    public class ResevatedTableRepository : Repository<Resevated_table>, IResevatedTableRepository
    {
        public ResevatedTableRepository(RestaurantModelContext context) : base(context)
        {
        }
        public RestaurantModelContext RestaurantModelContext
        {
            get { return Context as RestaurantModelContext; }
        }
    }
}