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
using System.Web;

namespace Aqua.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private ICustomerRepo _customerRepo;
        private ILocationRepo _locationRepo;
        private IOrderRepo _orderRepo;
        private IAnimalRepo _animalRepo;
        public OrderController(ICustomerRepo customerRepo, ILocationRepo locationRepo, IOrderRepo orderRepo, IAnimalRepo animalRepo)
        {
            _customerRepo = customerRepo;
            _locationRepo = locationRepo;
            _orderRepo = orderRepo;
            _animalRepo = animalRepo;
        }

        // GET: Order
        public ActionResult Index(string searchString)
        {
            List<Order> result = _orderRepo.GetAllOrders();
            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.FindAll(s => s.Customer.Email.Contains(searchString));
            }
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
        public ActionResult Create(OrderViewModel orderViewModel)
        {
            var currentLocation = _locationRepo.GetLocationById(orderViewModel.Location);
            var currentCustomer = _customerRepo.GetCustomerById(orderViewModel.Customer);
            var result = new Order();
            result.Location = currentLocation;
            result.Customer = currentCustomer;
            result.Date = DateTime.Now;
            result.Total = orderViewModel.Total;
            foreach (var orderItem in orderViewModel.OrderItems)
            {
                var animal = _animalRepo.GetAnimalById(orderItem.AnimalId);
                var newItem = new OrderItem(0, animal, orderItem.Quantity, orderItem.Total);
                result.OrderItems.Add(newItem);
            }
            var resultOrder = _orderRepo.CreateOrderEntityReturnIt(result);
            return RedirectToAction("AddOrderItem", new { Id = resultOrder.Id });
        }

        //public ActionResult AddOrderItem(int id)
        //{
        //    var result = new OrderViewModel(_orderRepo.GetOrderById(id));
        //    var location = _locationRepo.GetLocationById(result.Location);
        //    foreach(var animal in location.Inventory)
        //    {
        //        var currentAnimal = _animalRepo.GetAnimalByName(animal.AnimalName);
        //        result.Animals.Add(currentAnimal);
        //    }
        //    return View(result);
        //}
        public ActionResult AddOrderItem(int id)
        {
            var result = new OrderItemViewModel();
            result.OrderId = id;
            result.Quantity = 1;
            var currentOrder = _orderRepo.GetOrderById(id);
            var currentInventory = _locationRepo.GetInvByLocation(currentOrder.Location);
            var animals = _animalRepo.GetAllAnimals();
            foreach (var animal in animals)
            {
                if (currentInventory.Any(a => (a.AnimalName == animal.Name && a.Quantity > 0))) // If this animal exists in the location's inventory it is added as an option to buy
                {
                    result.Animals.Add(animal);
                }
            }
            if(TempData["QuantityError"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["QuantityError"].ToString());
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrderItem(OrderItemViewModel orderItem)
        {
            if (ModelState.IsValid)
            {
                var order = _orderRepo.GetOrderById(orderItem.OrderId); // Get order ID of the first item and route to it afterwards since all items are in the same order
                var animal = _animalRepo.GetAnimalById(orderItem.AnimalId);
                var locationInventory = _locationRepo.GetInvByLocation(order.Location);
                var invItem = locationInventory.Find(i => i.AnimalName == animal.Name);
                if (invItem.Quantity - orderItem.Quantity <= 0) // Check to see if order quantity is less than the quantity of animals in stock
                {
                    TempData["QuantityError"] = $"Quantity is too high, not enough {animal.Name}(s) in inventory. Currently have {invItem.Quantity} {animal.Name}(s) in stock.";
                    return RedirectToAction("AddOrderItem", new { OrderId = orderItem.OrderId });
                }
                else
                {
                    bool existInOrder = order.OrderItems.Any(o => o.Animal.Id == animal.Id);
                    var total = animal.Price * orderItem.Quantity;
                    if (existInOrder) // Animal already exists in order
                    {
                        foreach (var thing in order.OrderItems)
                        {
                            if (thing.Animal.Id == orderItem.AnimalId) // Loop through all order items in the current order until an animal id matches the one that is in the current order
                            {
                                var existingOrder = _orderRepo.GetOrderItemById(thing.Id);
                                existingOrder.Quantity += orderItem.Quantity;
                                existingOrder.Total += (decimal)total;
                                order.Total += (decimal)total;
                                _orderRepo.UpdateOrderItemEntity(existingOrder);
                                _orderRepo.UpdateOrderEntity(order);
                            }
                        };
                    }
                    else
                    {
                        var newItem = new OrderItem(orderItem.OrderId, animal, orderItem.Quantity, (decimal)total);
                        order.Total += (decimal)total;
                        _orderRepo.CreateOrderItemEntity(newItem);
                        _orderRepo.UpdateOrderEntity(order);
                    }
                    invItem.Quantity -= orderItem.Quantity; // Subtract quantity of order from inventory
                    _locationRepo.UpdateInventoryEntity(invItem.LocationId, invItem.AnimalName, invItem.Quantity);
                    return RedirectToAction("AddOrderItem", new { OrderId = order.Id });
                }
            }
            else
            {
                return RedirectToAction("AddOrderItem", new { OrderId = orderItem.OrderId });
            }
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult AddOrderItemToList([Bind("Id, Location, OrderItems")] OrderViewModel order)
        //{
        //    var result = new OrderItemViewModel();
        //    result.OrderId = order.Id;
        //    result.Quantity = 1;
        //    var currentOrder = _orderRepo.GetOrderById(order.Id);
        //    var location = _locationRepo.GetLocationById(currentOrder.Location.Id);
        //    foreach (var animal in location.Inventory)
        //    {
        //        var currentAnimal = _animalRepo.GetAnimalByName(animal.AnimalName);
        //        result.Animals.Add(currentAnimal);
        //    }
        //    order.OrderItems.Add(result);
        //    return PartialView("OrderItemList", order);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddOrderItem([Bind("Id, Location, Animals, OrderItems")] OrderViewModel order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var orderItem in order.OrderItems)
        //        {
        //            var animal = _animalRepo.GetAnimalById(orderItem.AnimalId);
        //            var location = _locationRepo.GetLocationById(order.Location);
        //            var locationInventory = _locationRepo.GetInvByLocation(location);
        //            var invItem = locationInventory.Find(i => i.AnimalName == animal.Name);
        //            if (invItem.Quantity - orderItem.Quantity <= 0) // Check to see if order quantity is less than the quantity of animals in stock
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                bool existInOrder = order.OrderItems.Any(o => o.AnimalId == animal.Id);
        //                var total = animal.Price * orderItem.Quantity;

        //                    var newItem = new OrderItem(orderItem.OrderId, animal, orderItem.Quantity, (decimal)total);
        //                    order.Total += (decimal)total;
        //                    var orderEntity = _orderRepo.GetOrderById(order.Id);
        //                    orderEntity.Total += (decimal)total;
        //                    _orderRepo.CreateOrderItemEntity(newItem);
        //                    _orderRepo.UpdateOrderEntity(orderEntity);
        //                invItem.Quantity -= orderItem.Quantity; // Subtract quantity of order from inventory
        //                _locationRepo.UpdateInventoryEntity(invItem.LocationId, invItem.AnimalName, invItem.Quantity);
        //            }
        //        }
        //        return RedirectToAction("Details", new { id = order.Id });
        //    }
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddOrderItemFromDetails(OrderViewModel order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var orderItem in order.OrderItems)
        //        {
        //            var animal = _animalRepo.GetAnimalById(orderItem.AnimalId);
        //            var location = _locationRepo.GetLocationById(order.Location);
        //            var locationInventory = _locationRepo.GetInvByLocation(location);
        //            var invItem = locationInventory.Find(i => i.AnimalName == animal.Name);
        //            if (invItem.Quantity - orderItem.Quantity <= 0) // Check to see if order quantity is less than the quantity of animals in stock
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                bool existInOrder = order.OrderItems.Any(o => o.AnimalId == animal.Id);
        //                var total = animal.Price * orderItem.Quantity;
        //                if (existInOrder) // Animal already exists in order
        //                {
        //                    foreach (var thing in order.OrderItems)
        //                    {
        //                        if (thing.AnimalId == orderItem.AnimalId) // Loop through all order items in the current order until an animal id matches the one that is in the current order
        //                        {
        //                            var existingOrderItem = _orderRepo.GetOrderItemById(thing.Id);
        //                            existingOrderItem.Quantity += orderItem.Quantity;
        //                            existingOrderItem.Total += (decimal)total;
        //                            var orderEntity = _orderRepo.GetOrderById(order.Id);
        //                            orderEntity.Total += (decimal)total;
        //                            _orderRepo.UpdateOrderItemEntity(existingOrderItem);
        //                            _orderRepo.UpdateOrderEntity(orderEntity);
        //                        }
        //                    };
        //                }
        //                else
        //                {
        //                    var newItem = new OrderItem(orderItem.OrderId, animal, orderItem.Quantity, (decimal)total);
        //                    order.Total += (decimal)total;
        //                    var orderEntity = _orderRepo.GetOrderById(order.Id);
        //                    orderEntity.Total += (decimal)total;
        //                    _orderRepo.CreateOrderItemEntity(newItem);
        //                    _orderRepo.UpdateOrderEntity(orderEntity);
        //                }
        //                invItem.Quantity -= orderItem.Quantity; // Subtract quantity of order from inventory
        //                _locationRepo.UpdateInventoryEntity(invItem.LocationId, invItem.AnimalName, invItem.Quantity);
        //            }
        //        }
        //        return RedirectToAction("Details", new { id = order.Id });
        //    }
        //    return View();
        //}

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