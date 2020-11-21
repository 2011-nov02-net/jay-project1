using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Aqua.Data.Model;

namespace Aqua.Data
{
    public class LocationRepo
    {
        private readonly DbContextOptions<AquaContext> _contextOptions;

        public LocationRepo(DbContextOptions<AquaContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        public ICollection<LocationEntity> GetAllLocations()
        {
            using var context = new AquaContext(_contextOptions);
            var dbLocations = context.Locations.ToList();
            return dbLocations;
        }
    }
}
