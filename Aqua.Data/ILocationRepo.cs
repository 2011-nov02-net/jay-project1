using Aqua.Library;
using System.Collections.Generic;

namespace Aqua.Data
{
    public interface ILocationRepo
    {
        List<Location> GetAllLocations();
        Location GetLocationByCity(string city);
        Location GetLocationById(int id);
        List<InventoryItem> GetInvByLocation(Location location);
        InventoryItem GetInventoryById(int? id);
        void CreateInventoryEntity(Location location, Animal animal, int stock);
        void CreateLocationEntity(Location location);
        void UpdateInventoryEntity(int locationId, string animalName, int stock);
        void UpdateLocationEntity(Location location);
        void DeleteLocationEntity(Location location);
    }
}
