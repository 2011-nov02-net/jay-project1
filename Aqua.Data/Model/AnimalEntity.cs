using System;
using System.Collections.Generic;

namespace Aqua.Data.Model
{
    public class AnimalEntity
    {
        public AnimalEntity()
        {
            Inventory = new HashSet<InventoryItemEntity>();
            OrderItems = new HashSet<OrderItemEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public virtual ICollection<InventoryItemEntity> Inventory { get; set; }
        public virtual ICollection<OrderItemEntity> OrderItems { get; set; }
    }
}
