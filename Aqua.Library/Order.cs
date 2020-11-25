using System;
using System.Collections.Generic;

namespace Aqua.Library
{
    public class Order
    {
        public Order()
        {
        }
        public Order(int locationId, string email, decimal total){
            LocationId = locationId;
            Email = email;
            Total = total;
            OrderItems = new List<OrderItem>();
        }
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Email { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}