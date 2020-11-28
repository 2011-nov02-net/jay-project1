using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Library;
using System.ComponentModel.DataAnnotations;

namespace Aqua.WebApp.Models
{
    public class LocationViewModel
    {

        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(2048)]
        public string City { get; set; }
        public List<InventoryItem> Inventory { get; set; }
    }
}
