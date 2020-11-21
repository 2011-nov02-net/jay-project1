using System;
using System.Collections.Generic;
using System.Text;

namespace Aqua.Library
{
    public class InventoryItem
    {
        public InventoryItem(int id, int locationId, string animalName, int quantity)
        {
            Id = id;
            LocationId = locationId;
            AnimalName = animalName;
            Quantity = quantity;
        }
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string AnimalName { get; set; }
        public int Quantity { get; set; }
    }
}