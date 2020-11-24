using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public LocationController(DbContextOptions<AquaContext> context)
        {
            _locationRepo = new LocationRepo(context);
            _animalRepo = new AnimalRepo(context);
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
        public IActionResult Details(int? id)
        {
            if (id == null)
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
        public IActionResult ImportAnimal()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportAnimal(InventoryItem inventoryItem)
        {
            var location = _locationRepo.GetLocationById(inventoryItem.LocationId);
                if (ModelState.IsValid)
                {
                    var newAnimal = _animalRepo.GetAnimalByName(inventoryItem.AnimalName);
                    _locationRepo.CreateInventoryEntity(location, newAnimal, inventoryItem.Quantity);
                    return RedirectToAction(nameof(Index));
                };

            return View();
        }

        // GET: Location/Edit/1
        public IActionResult Edit(int? id)
        {
            if (id == null)
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

        // // // GET: Location/Delete/1
        public IActionResult Delete(int? id)
        {
            if (id == null)
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
            bool exist = (_locationRepo.GetLocationById(id) != null);
            return exist;
        }
    }
}
