using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aqua.WebApp.Models
{
    public class InventoryViewModel
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string AnimalName { get; set; }
        public int Quantity { get; set; }
    }
}