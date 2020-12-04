using Aqua.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [DataType(DataType.Text)]
        public int LocationId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string AnimalName { get; set; }
        [Required]
        [Range(0, 100000)]
        public int Quantity { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
