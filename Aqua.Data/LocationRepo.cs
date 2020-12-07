using Aqua.Data.Model;
using Aqua.Library;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Aqua.Data
{
    public class LocationRepo : ILocationRepo
    {
        private readonly DbContextOptions<AquaContext> _contextOptions;

        public LocationRepo(DbContextOptions<AquaContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        public List<Location> GetAllLocations()
        {
            using AquaContext context = new AquaContext(_contextOptions);
            List<LocationEntity> dbLocations = context.Locations.Distinct().ToList();
            List<Location> result = new List<Location>();
            foreach (LocationEntity location in dbLocations)
            {
                Location newLocation = new Location()
                {
                    Id = location.Id,
                    City = location.City
                };
                result.Add(newLocation);
            };
            return result;
        }
        public Location GetLocationByCity(string city)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            LocationEntity dbLocation = context.Locations
                .FirstOrDefault(l => l.City == city);
            if (dbLocation != null)
            {
                Location result = new Location()
                {
                    Id = dbLocation.Id,
                    City = dbLocation.City
                };
                List<InventoryItem> resultInv = GetInvByLocation(result);
                foreach (InventoryItem thing in resultInv)
                {
                    result.Inventory.Add(thing);
                }
                return result;
            }
            else
            {
                return null;
            }
        }
        public Location GetLocationById(int id)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            LocationEntity dbLocation = context.Locations
                .Where(l => l.Id == id)
                .FirstOrDefault();
            if (dbLocation == null)
            {
                return null;
            }
            else
            {
                Location result = new Location()
                {
                    Id = dbLocation.Id,
                    City = dbLocation.City
                };
                List<InventoryItem> resultInv = GetInvByLocation(result);
                foreach (InventoryItem thing in resultInv)
                {
                    result.Inventory.Add(thing);
                }
                return result;
            }
        }
        public List<InventoryItem> GetInvByLocation(Location location)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            List<InventoryItemEntity> dbInventory = context.Inventories
                .Where(i => i.LocationId == location.Id)
                .Include(i => i.Animal)
                .ToList();
            List<InventoryItem> result = new List<InventoryItem>();
            foreach (InventoryItemEntity item in dbInventory)
            {
                InventoryItem newItem = new InventoryItem(item.LocationId, item.Animal.Name, item.Quantity)
                {
                    Id = item.Id
                };
                result.Add(newItem);
            }
            return result;
        }
        public InventoryItem GetInventoryById(int? id)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            InventoryItemEntity dbInventory = context.Inventories
                .Where(i => i.Id == id)
                .Include(i => i.Animal)
                .FirstOrDefault();
            if (dbInventory == null)
            {
                return null;
            }
            else
            {
                InventoryItem result = new InventoryItem()
                {
                    Id = dbInventory.Id,
                    LocationId = dbInventory.LocationId,
                    AnimalName = dbInventory.Animal.Name,
                    Quantity = dbInventory.Quantity
                };
                return result;
            }
        }
        public void CreateInventoryEntity(Location location, Animal animal, int stock)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            Location currentLocation = location;
            Animal currentAnimal = animal;
            InventoryItemEntity newEntry = new InventoryItemEntity()
            {
                LocationId = currentLocation.Id,
                AnimalId = currentAnimal.Id,
                Quantity = stock
            };
            context.Inventories.Add(newEntry);
            context.SaveChanges();
        }
        public void CreateLocationEntity(Location location)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            LocationEntity newEntry = new LocationEntity()
            {
                City = location.City
            };
            context.Locations.Add(newEntry);
            context.SaveChanges();
        }
        public void UpdateInventoryEntity(int locationId, string animalName, int stock)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            InventoryItemEntity dbInventory = context.Inventories
                .Include(i => i.Animal)
                .Where(i => i.LocationId == locationId && i.Animal.Name == animalName)
                .FirstOrDefault();
            dbInventory.Quantity = stock;
            context.SaveChanges();
        }
        public void UpdateLocationEntity(Location location)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            LocationEntity dbLocation = context.Locations
                .Where(i => i.Id == location.Id)
                .FirstOrDefault();
            dbLocation.City = location.City;
            context.SaveChanges();
        }
        public void DeleteLocationEntity(Location location)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            LocationEntity dbLocation = context.Locations
                .Where(i => i.Id == location.Id)
                .FirstOrDefault();
            context.Remove(dbLocation);
            context.SaveChanges();
        }
    }
}
