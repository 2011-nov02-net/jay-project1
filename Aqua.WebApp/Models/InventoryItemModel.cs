using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Library;

namespace Aqua.WebApp.Models
{
    public class InventoryItemModel
    {
        public InventoryItemModel()
        {
            Animals = new List<Animal>();
        }
        public InventoryItemModel(InventoryItem inventoryItem)
        {
            Id = inventoryItem.Id;
            LocationId = inventoryItem.LocationId;
            AnimalName = inventoryItem.AnimalName;
            Quantity = inventoryItem.Quantity;
            Animals = new List<Animal>();
        }
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string AnimalName { get; set; }
        public int Quantity { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
