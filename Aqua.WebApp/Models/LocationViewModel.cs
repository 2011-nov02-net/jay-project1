using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Library;

namespace Aqua.WebApp.Models
{
    public class LocationViewModel
    {
        public int Id { get; set; }
        public string City { get; set; }
        public List<InventoryItem> Inventory { get; set; }
    }
}
