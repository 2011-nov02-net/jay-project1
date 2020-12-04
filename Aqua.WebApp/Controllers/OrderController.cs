using Aqua.Data;
using Aqua.Library;
using Aqua.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Aqua.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly ILocationRepo _locationRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IAnimalRepo _animalRepo;
        public OrderController(ICustomerRepo customerRepo, ILocationRepo locationRepo, IOrderRepo orderRepo, IAnimalRepo animalRepo)
        {
            _customerRepo = customerRepo;
            _locationRepo = locationRepo;
            _orderRepo = orderRepo;
            _animalRepo = animalRepo;
        }

        // GET: Order
        public IActionResult Index(string searchString)
        {
            List<Order> result = _orderRepo.GetAllOrders();
            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.FindAll(s => s.Customer.Email.Contains(searchString));
            }
            return View(result);
        }

        // GET: Order/Details/5
        public IActionResult Details(int id)
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
            else
            {
                var result = _orderRepo.GetOrderById(id);
                return View(result);
            }
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
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
        public IActionResult Create(OrderViewModel orderViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
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

        public IActionResult AddOrderItem(int id)
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
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
            if (TempData["QuantityError"] != null)
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

        public IActionResult Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
            return View();
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Order order)
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
            if (!ModelState.IsValid)
            {
                return Error();
            }
            var order = _orderRepo.GetOrderById(id);
            if (order == null)
            {
                return Error();
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
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}