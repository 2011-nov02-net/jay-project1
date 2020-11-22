using System;
using System.Collections.Generic;
using System.Text;

namespace Aqua.Library
{
    public class OrderItem
    {
        public OrderItem(int orderId, int animalId, int quantity, decimal total)
        {
            OrderId = orderId;
            AnimalId = animalId;
            Quantity = quantity;
            Total = total;
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int AnimalId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
