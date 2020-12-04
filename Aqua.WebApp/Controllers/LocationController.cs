using Aqua.Data;
using Aqua.Library;
using Aqua.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Aqua.WebApp.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationRepo _locationRepo;
        private readonly IAnimalRepo _animalRepo;
        private readonly IOrderRepo _orderRepo;
        public LocationController(ILocationRepo locationRepo, IAnimalRepo animalRepo, IOrderRepo orderRepo)
        {
            _locationRepo = locationRepo;
            _animalRepo = animalRepo;
            _orderRepo = orderRepo;
        }

        // GET: Location
        public IActionResult Index()
        {
            var result = _locationRepo.GetAllLocations().Select(x => new LocationViewModel
            {
                Id = x.Id,
                City = x.City
            });
            return View(result);
        }

        // GET: Location/Details/1
        public IActionResult Details(int id)
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
            var location = _locationRepo.GetLocationById(id);
            if (location == null)
            {
                return Error();
            }
            TempData["LocationId"] = location.Id;
            return View(location);
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            if (TempData["CityExistError"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["CityExistError"].ToString());
            }
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Location location)
        {
            if (LocationCityExists(location.City))
            {
                TempData["CityExistError"] = $"Location '{location.City}' already exists.";
                return RedirectToAction(nameof(Create));
            }
            else
            {
                _locationRepo.CreateLocationEntity(location);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Location/CreateInventoryItem
        public IActionResult ImportAnimal(int id)
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
            var result = new InventoryItemModel();
            var animalList = _animalRepo.GetAllAnimals();
            result.LocationId = id;
            result.Quantity = 1;
            foreach (var animal in animalList)
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
            else
            {
                return Error();
            }
            return View();
        }

        // GET: Location/Edit/1
        public IActionResult Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
            if (TempData["CityExistError"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["CityExistError"].ToString());
            }
            var location = _locationRepo.GetLocationById(id);
            if (location == null)
            {
                return Error();
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
            if (LocationCityExists(location.City))
            {
                TempData["CityExistError"] = $"Location '{location.City}' already exists.";
                return RedirectToAction("Edit", new { id = location.Id });
            }
            else if (ModelState.IsValid)
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
            if (!ModelState.IsValid)
            {
                return Error();
            }
            var location = _locationRepo.GetLocationById(id);
            if (location == null)
            {
                return Error();
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
        private bool LocationCityExists(string city)
        {
            bool exist = (_locationRepo.GetLocationByCity(city) != null); // If it returns not null it exists, so true
            return exist;
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public bool AnimalExists(string name)
        {
            var animal = _animalRepo.GetAnimalByName(name);
            if (animal != null)
            {
                return true;
            }
            return false;
        }
    }
}
