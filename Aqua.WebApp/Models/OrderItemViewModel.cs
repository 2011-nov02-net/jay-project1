using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aqua.Library;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Aqua.WebApp.Utilities;

namespace Aqua.WebApp.Models
{
    public class OrderItemViewModel
    {
        public OrderItemViewModel()
        {
            Animals = new List<Animal>();
        }
        public OrderItemViewModel(OrderItem orderItem)
        {
            OrderId = orderItem.OrderId;
            AnimalId = orderItem.Animal.Id;
            Quantity = orderItem.Quantity;
            Total = orderItem.Total;
            Animals = new List<Animal>();
        }
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int AnimalId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Total { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
