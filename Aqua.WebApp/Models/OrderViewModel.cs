﻿using Aqua.Library;
using System;
using System.Collections.Generic;
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
            LocationCity = order.Location.City;
            Customer = order.Customer.Id;
            CustomerEmail = order.Customer.Email;
            Total = order.Total;
            Date = order.Date;
            LocationList = new List<Location>();
            CustomerList = new List<Customer>();
            Animals = new List<Animal>();
            OrderItems = new List<OrderItemViewModel>();
            foreach (var orderItem in order.OrderItems)
            {
                var newOrderItem = new OrderItemViewModel(orderItem);
                newOrderItem.AnimalName = orderItem.Animal.Name;
                OrderItems.Add(newOrderItem);
            };
        }
        public int Id { get; set; }
        [Required]
        public int Location { get; set; }
        public string LocationCity { get; set; }
        [Required]
        public int Customer { get; set; }
        public string CustomerEmail { get; set; }
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public List<Location> LocationList { get; set; }
        public List<Customer> CustomerList { get; set; }
        public List<Animal> Animals { get; set; }
    }
}
