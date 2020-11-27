using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Library;

namespace Aqua.WebApp.Models
{
    public class OrderItemViewModel
    {
        public OrderItemViewModel()
        {
            Animals = new List<Animal>();
        }
        public OrderItemViewModel(int orderId, Animal animal, int quantity, decimal total)
        {
            OrderId = orderId;
            Animal = animal;
            Quantity = quantity;
            Total = total;
            Animals = new List<Animal>();
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Animal Animal { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
