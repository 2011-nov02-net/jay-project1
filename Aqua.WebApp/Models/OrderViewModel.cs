﻿using System;
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
            OrderItems = new List<OrderItemViewModel>();
            LocationList = new List<Location>();
            CustomerList = new List<Customer>();
            Animals = new List<Animal>();
        }
        public int Id { get; set; }
        public int Location { get; set; }
        public int Customer { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public List<Location> LocationList { get; set; }
        public List <Customer> CustomerList { get; set; }
        public List<Animal> Animals { get; set; }
    }
}