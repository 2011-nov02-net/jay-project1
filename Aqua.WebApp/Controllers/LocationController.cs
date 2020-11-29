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
    public class LocationController : Controller
    {
        private LocationRepo _locationRepo;
        private AnimalRepo _animalRepo;
        private OrderRepo _orderRepo;
        public LocationController(DbContextOptions<AquaContext> context)
        {
            _locationRepo = new LocationRepo(context);
            _animalRepo = new AnimalRepo(context);
            _orderRepo = new OrderRepo(context);
        }

        // GET: Location
        public IActionResult Index()
        {
            var result = _locationRepo.GetAllLocations().Select(x => new LocationViewModel { 
                Id = x.Id,
                City = x.City
            });
            return View(result);
        }

        // GET: Location/Details/1
        public IActionResult Details(int id)
        {
            if (id < 0)
            {
                return NotFound();
            }

            var location = _locationRepo.GetLocationById(id);
            if (location == null)
            {
                return NotFound();
            }
            TempData["LocationId"] = location.Id;
            return View(location);
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,City")] Location location)
        {
            if (ModelState.IsValid)
            {
                _locationRepo.CreateLocationEntity(location);
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Location/CreateInventoryItem
        public IActionResult ImportAnimal(int id)
        {
            var result = new InventoryItemModel();
            var animalList = _animalRepo.GetAllAnimals();
            result.LocationId = id;
            result.Quantity = 1;
            foreach(var animal in animalList)
            {
                var newAnimal = new Animal()
                {
                    Id = animal.Id,
                    Name = animal.Name,
                    Price = animal.Price
                };
                result.Animals.Add(newAnimal);
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportAnimal(InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                var location = _locationRepo.GetLocationById(inventoryItem.LocationId);
                var animalCheck = AnimalExists(inventoryItem.AnimalName);
                if (animalCheck != false) // Check to see if animal exists in database already
                {
                    var animal = _animalRepo.GetAnimalByName(inventoryItem.AnimalName);
                    var existInventory = location.Inventory.Any(i => i.AnimalName == inventoryItem.AnimalName);
                    if (existInventory) // Check to see if animal exists in location inventory already
                    {
                        var invItem = location.Inventory.Find(i => i.AnimalName == inventoryItem.AnimalName);
                        invItem.Quantity += inventoryItem.Quantity;
                        _locationRepo.UpdateInventoryEntity(location.Id, animal.Name, invItem.Quantity);
                        return RedirectToAction("Details", new { id = location.Id });
                    }
                    else
                    {
                        _locationRepo.CreateInventoryEntity(location, animal, inventoryItem.Quantity);
                        return RedirectToAction("Details", new { id = location.Id });
                    }
                }
            }
            return View();
        }

        // GET: Location/Edit/1
        public IActionResult Edit(int id)
        {
            if (id < 0)
            {
                return NotFound();
            }

            var location = _locationRepo.GetLocationById(id);
            if (location == null)
            {
                return NotFound();
            }
            var result = new LocationViewModel(location);
            return View(result);
        }

        // POST: Location/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,City")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _locationRepo.UpdateLocationEntity(location);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (LocationExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }
        public IActionResult AddToInventory(int id)
        {
            if (id < 0)
            {
                return NotFound();
            }

            var inventoryItem = _locationRepo.GetInventoryById(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }
            var result = new InventoryItemModel(inventoryItem);
            return View(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToInventory(InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                var location = _locationRepo.GetLocationById(inventoryItem.LocationId);
                var invItem = location.Inventory.Find(i => i.AnimalName == inventoryItem.AnimalName);
                invItem.Quantity += inventoryItem.Quantity;
                _locationRepo.UpdateInventoryEntity(inventoryItem.LocationId, inventoryItem.AnimalName, invItem.Quantity);
                return RedirectToAction("Details", new { id = inventoryItem.LocationId });
            }
            return View();
        }

        public IActionResult LocationOrders(int id)
        {
            var location = _locationRepo.GetLocationById(id);
            List<Order> result = _orderRepo.GetOrdersByLocation(location);
            return View(result);
        }
        // // // GET: Location/Delete/1
        public IActionResult Delete(int id)
        {
            if (id < 0)
            {
                return NotFound();
            }

            var location = _locationRepo.GetLocationById(id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Location/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var location = _locationRepo.GetLocationById(id);
            _locationRepo.DeleteLocationEntity(location);
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            try
            {
                _locationRepo.GetLocationById(id);
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public bool AnimalExists(string name)
        {
            var animal = _animalRepo.GetAnimalByName(name);
            if(animal != null)
            {
                return true;
            }
            return false;
        }
    }
}
