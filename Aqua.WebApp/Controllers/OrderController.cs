using Aqua.Data;
using Aqua.Library;
using Aqua.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<OrderController> _logger;
        public OrderController(ILogger<OrderController> logger, ICustomerRepo customerRepo, ILocationRepo locationRepo, IOrderRepo orderRepo, IAnimalRepo animalRepo)
        {
            _logger = logger;
            _customerRepo = customerRepo;
            _locationRepo = locationRepo;
            _orderRepo = orderRepo;
            _animalRepo = animalRepo;
        }

        // GET: Order
        public IActionResult Index(string searchString)
        {
            List<Order> orderList = _orderRepo.GetAllOrders();
            List<OrderViewModel> result = new List<OrderViewModel>();
            foreach (Order ord in orderList)
            {
                OrderViewModel newOrd = new OrderViewModel(ord);
                result.Add(newOrd);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.FindAll(s => s.CustomerEmail.Contains(searchString, StringComparison.OrdinalIgnoreCase));
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
                Order result = _orderRepo.GetOrderById(id);
                if (result == null)
                {
                    return Error();
                }
                else
                {
                    return View(result);
                }
            }
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
            OrderViewModel result = new OrderViewModel();
            List<Location> locationList = _locationRepo.GetAllLocations();
            List<Customer> customerList = _customerRepo.GetAllCustomers();
            foreach (Location location in locationList)
            {
                result.LocationList.Add(location);
            }
            foreach (Customer customer in customerList)
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
            Location currentLocation = _locationRepo.GetLocationById(orderViewModel.Location);
            Customer currentCustomer = _customerRepo.GetCustomerById(orderViewModel.Customer);
            Order result = new Order
            {
                Location = currentLocation,
                Customer = currentCustomer,
                Date = DateTime.Now,
                Total = orderViewModel.Total
            };
            foreach (OrderItemViewModel orderItem in orderViewModel.OrderItems)
            {
                Animal animal = _animalRepo.GetAnimalById(orderItem.AnimalId);
                OrderItem newItem = new OrderItem(0, animal, orderItem.Quantity, orderItem.Total);
                result.OrderItems.Add(newItem);
            }
            Order resultOrder = _orderRepo.CreateOrderEntityReturnIt(result);
            return RedirectToAction("AddOrderItem", new { Id = resultOrder.Id });
        }

        public IActionResult AddOrderItem(int id)
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
            OrderItemViewModel result = new OrderItemViewModel
            {
                OrderId = id,
                Quantity = 1
            };
            Order currentOrder = _orderRepo.GetOrderById(id);
            List<InventoryItem> currentInventory = _locationRepo.GetInvByLocation(currentOrder.Location);
            List<Animal> animals = _animalRepo.GetAllAnimals();
            foreach (Animal animal in animals)
            {
                if (currentInventory.Any(a => (a.AnimalName == animal.Name && a.Quantity > 0))) // If this animal exists in the location's inventory it is added as an option to buy
                {
                    result.Animals.Add(animal);
                }
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrderItem(OrderItemViewModel orderItem)
        {
            if (ModelState.IsValid)
            {
                Order order = _orderRepo.GetOrderById(orderItem.OrderId); // Get order ID of the first item and route to it afterwards since all items are in the same order
                Animal animal = _animalRepo.GetAnimalById(orderItem.AnimalId);
                List<InventoryItem> locationInventory = _locationRepo.GetInvByLocation(order.Location);
                InventoryItem invItem = locationInventory.Find(i => i.AnimalName == animal.Name);
                if (invItem.Quantity - orderItem.Quantity < 0) // Check to see if order quantity is less than the quantity of animals in stock
                {
                    TempData["QuantityError"] = $"Error. Quantity is too high, not enough {animal.Name}(s) in inventory. Currently have {invItem.Quantity} {animal.Name}(s) in stock.";
                    return RedirectToAction("AddOrderItem", new { OrderId = orderItem.OrderId });
                }
                else
                {
                    bool existInOrder = order.OrderItems.Any(o => o.Animal.Id == animal.Id);
                    double total = animal.Price * orderItem.Quantity;
                    if (existInOrder) // Animal already exists in order
                    {
                        foreach (OrderItem thing in order.OrderItems)
                        {
                            if (thing.Animal.Id == orderItem.AnimalId) // Loop through all order items in the current order until an animal id matches the one that is in the current order
                            {
                                OrderItem existingOrder = _orderRepo.GetOrderItemById(thing.Id);
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
                        OrderItem newItem = new OrderItem(orderItem.OrderId, animal, orderItem.Quantity, (decimal)total);
                        order.Total += (decimal)total;
                        _orderRepo.CreateOrderItemEntity(newItem);
                        _orderRepo.UpdateOrderEntity(order);
                    }
                    invItem.Quantity -= orderItem.Quantity; // Subtract quantity of order from inventory
                    _locationRepo.UpdateInventoryEntity(invItem.LocationId, invItem.AnimalName, invItem.Quantity);
                    TempData["AddOrderItemSuccess"] = $"Success! Added {orderItem.Quantity} {animal.Name}(s) to your order.";
                    return RedirectToAction("AddOrderItem", new { OrderId = order.Id });
                }
            }
            else
            {
                return Error();
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
            Order order = _orderRepo.GetOrderById(id);
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
            Order order = _orderRepo.GetOrderById(id);
            _orderRepo.DeleteOrderEntity(order);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Error()
        {
            _logger.LogError($"Error in order controller");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}