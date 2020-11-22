using System;
using System.Collections.Generic;
using System.Text;

namespace Aqua.Library
{
    public class Location
    {
        public Location(string city)
        {
            City = city;
            Inventory = new List<InventoryItem>();
        }
        public int Id { get; set; }
        public string City { get; set; }
        public  List<InventoryItem> Inventory { get; set; }
    }
}