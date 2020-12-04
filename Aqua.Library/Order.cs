using System;
using System.Collections.Generic;

namespace Aqua.Library
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        public Order(Location location, Customer customer, decimal total)
        {
            Location = location;
            Customer = customer;
            Total = total;
            OrderItems = new List<OrderItem>();
        }
        public int Id { get; set; }
        public Location Location { get; set; }
        public Customer Customer { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}