using System;
using System.Collections.Generic;

namespace Aqua.Data.Model
{
    public class LocationEntity
    {
        public LocationEntity()
        {
            Inventory = new HashSet<InventoryItemEntity>();
            Orders = new HashSet<OrderEntity>();
        }
        public int Id { get; set; }
        public string City { get; set; }
        public virtual ICollection<InventoryItemEntity> Inventory { get; set; }
        public virtual ICollection<OrderEntity> Orders { get; set; }
    }
}
