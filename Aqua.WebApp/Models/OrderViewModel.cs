using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Library;

namespace Aqua.WebApp.Models
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            OrderItems = new List<OrderItem>();
            LocationList = new List<Location>();
            CustomerList = new List<Customer>();
        }
        public OrderViewModel(Location location, Customer customer, decimal total)
        {
            Location = location;
            Customer = customer;
            Total = total;
            OrderItems = new List<OrderItem>();
            LocationList = new List<Location>();
            CustomerList = new List<Customer>();
        }
        public int Id { get; set; }
        public Location Location { get; set; }
        public Customer Customer { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Location> LocationList { get; set; }
        public List <Customer> CustomerList { get; set; }
    }
}
