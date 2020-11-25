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
        }
        public InventoryItemModel(int locationId, string animalName, int quantity)
        {
            LocationId = locationId;
            AnimalName = animalName;
            Quantity = quantity;
            Animals = new List<Animal>();
        }
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string AnimalName { get; set; }
        public int Quantity { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
