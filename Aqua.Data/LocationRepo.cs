using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Aqua.Data.Model;
using Aqua.Library;

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
        public Location GetLocationByCity(string city) {
            using var context = new AquaContext(_contextOptions);
            var dbLocation = context.Locations
                .FirstOrDefault(l => l.City == city);
            var result = new Location(dbLocation.City);
            result.Id = dbLocation.Id;
            var resultInv = GetInvByLocation(result);
            foreach (var thing in resultInv)
            {
                result.Inventory.Add(thing);
            }
            return result;
        }
        public List<InventoryItem> GetInvByLocation(Location location)
        {
            using var context = new AquaContext(_contextOptions);
            var dbInventory = context.Inventories
                .Where(i => i.LocationId == location.Id)
                .Include(i => i.Animal)
                .ToList();
            var result = new List<InventoryItem>();
            foreach(var item in dbInventory)
            {
                var newItem = new InventoryItem(item.LocationId, item.Animal.Name, item.Quantity);
                newItem.Id = item.Id;
            }
            return result;
        }
        public void CreateInventoryEntity(Location location, Animal animal, int stock)
        {
            using var context = new AquaContext(_contextOptions);
            var currentLocation = location;
            var currentAnimal = animal;
            var newEntry = new InventoryItemEntity()
            {
                LocationId = currentLocation.Id,
                AnimalId = currentAnimal.Id,
                Quantity = stock
            };
            context.Inventories.Add(newEntry);
            context.SaveChanges();
        }
        public void UpdateInventoryEntity(Location location, Animal animal, int stock)
        {
            using var context = new AquaContext(_contextOptions);
            var currentLocation = GetLocationByCity(location.City);
            var dbInventory = context.Inventories
                .Include(i => i.Animal)
                .Where(i => i.LocationId == currentLocation.Id && i.Animal.Name == animal.Name)
                .FirstOrDefault();
            dbInventory.Quantity += stock;
            context.SaveChanges();
        }
    }
}
