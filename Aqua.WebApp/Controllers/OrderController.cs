﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Aqua.Data.Model;
using Aqua.Data;
using Aqua.Library;
using Aqua.WebApp.Models;

namespace Aqua.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private CustomerRepo _customerRepo;
        private LocationRepo _locationRepo;
        private OrderRepo _orderRepo;
        private AnimalRepo _animalRepo;
        public OrderController(DbContextOptions<AquaContext> context)
        {
            _customerRepo = new CustomerRepo(context);
            _locationRepo = new LocationRepo(context);
            _orderRepo = new OrderRepo(context);
            _animalRepo = new AnimalRepo(context);
        }

        // GET: Order
        public ActionResult Index()
        {
            List<Order> result = _orderRepo.GetAllOrders();
            return View(result);
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            var result = _orderRepo.GetOrderById(id);
            return View(result);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            var result = new OrderViewModel();
            var locationList = _locationRepo.GetAllLocations();
            var customerList = _customerRepo.GetAllCustomers();
            foreach (var location in locationList)
            {
                result.LocationList.Add(location);
            }
            foreach (var customer in customerList)
            {
                result.CustomerList.Add(customer);
            }
            result.Total = 0;
            return View(result);
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(OrderViewModel orderViewModel)
        {
            var currentLocation = _locationRepo.GetLocationById(orderViewModel.Location);
            var currentCustomer = _customerRepo.GetCustomerById(orderViewModel.Customer);
            var result = new Order();
            result.Location = currentLocation;
            result.Customer = currentCustomer;
            result.Date = DateTime.Now;
            result.Total = orderViewModel.Total;
            _orderRepo.CreateOrderEntity(result);
            return RedirectToAction("Index");
        }

        public ActionResult AddOrderItem(int id)
        {
            var result = new OrderItemViewModel();
            result.OrderId = id;
            result.Quantity = 1;
            var animals = _animalRepo.GetAllAnimals();
            foreach(var animal in animals)
            {
                result.Animals.Add(animal);
            }
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrderItem(OrderItemViewModel orderItem)
        {
            if (ModelState.IsValid)
            {
                var order = _orderRepo.GetOrderById(orderItem.OrderId);
                var animal = _animalRepo.GetAnimalById(orderItem.AnimalId);
                var existInOrder = order.OrderItems.Any(o => o.Animal.Id == animal.Id);
                var total = animal.Price * orderItem.Quantity;
                if (existInOrder) // Animal already exists in order
                {
                    var existingOrder = order.OrderItems.First(o => o.Animal.Id == animal.Id);
                    existingOrder.Quantity += orderItem.Quantity;
                    existingOrder.Total += (decimal)total;
                    order.Total += (decimal)total;
                    _orderRepo.UpdateOrderItemEntity(existingOrder);
                    _orderRepo.UpdateOrderEntity(order);
                    return RedirectToAction("Details", new { id = order.Id });
                }
                else
                {
                    var newItem = new OrderItem(orderItem.OrderId, animal, orderItem.Quantity, (decimal)total);
                    order.Total += (decimal)total;
                    _orderRepo.CreateOrderItemEntity(newItem);
                    _orderRepo.UpdateOrderEntity(order);
                    return RedirectToAction("Details", new { id = order.Id });
                }
            }
            return View();
        }
        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Order order)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            var order = _orderRepo.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = _orderRepo.GetOrderById(id);
            _orderRepo.DeleteOrderEntity(order);
            return RedirectToAction(nameof(Index));
        }
    }
}
