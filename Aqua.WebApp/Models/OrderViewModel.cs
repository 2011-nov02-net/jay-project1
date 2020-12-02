using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Library;
using System.ComponentModel.DataAnnotations;

namespace Aqua.WebApp.Models
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            OrderItems = new List<OrderItemViewModel>();
            LocationList = new List<Location>();
            CustomerList = new List<Customer>();
            Animals = new List<Animal>();
        }
        public OrderViewModel(Order order)
        {
            Id = order.Id;
            Location = order.Location.Id;
            Customer = order.Customer.Id;
            Total = order.Total;
            LocationList = new List<Location>();
            CustomerList = new List<Customer>();
            Animals = new List<Animal>();
            foreach(var orderItem in order.OrderItems){
                var newOrderItem = new OrderItemViewModel(orderItem);
                OrderItems.Add(newOrderItem);
            };
        }
        public int Id { get; set; }
        [Required]
        public int Location { get; set; }
        [Required]
        public int Customer { get; set; }
        public decimal Total { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public List<Location> LocationList { get; set; }
        public List <Customer> CustomerList { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
