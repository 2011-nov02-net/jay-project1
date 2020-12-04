using Aqua.Library;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aqua.WebApp.Models
{
    public class LocationViewModel
    {
        public LocationViewModel()
        {
        }
        public LocationViewModel(Location location)
        {
            Id = location.Id;
            City = location.City;
            Inventory = location.Inventory;
        }
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(2048)]
        public string City { get; set; }
        public List<InventoryItem> Inventory { get; set; }
    }
}
