using System;
using System.Collections.Generic;
using System.Text;

namespace Aqua.Library
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string AnimalName { get; set; }
        public int Quantity { get; set; }
    }
}