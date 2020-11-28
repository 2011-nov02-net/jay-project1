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
        public OrderItemViewModel(int orderId, int animalId, int quantity, decimal total)
        {
            OrderId = orderId;
            AnimalId = animalId;
            Quantity = quantity;
            Total = total;
            Animals = new List<Animal>();
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int AnimalId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
