using System;
using System.Collections.Generic;

namespace Aqua.Data.Model
{
    public class OrderEntity
    {
        public OrderEntity()
        {
            OrderItems = new HashSet<OrderItemEntity>();
        }
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }

        public virtual CustomerEntity Customer { get; set; }
        public virtual LocationEntity Location { get; set; }
        public virtual ICollection<OrderItemEntity> OrderItems { get; set; }
    }
}
