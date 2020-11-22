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

namespace Aqua.WebApp.Controllers
{
    public class LocationController : Controller
    {
        private LocationRepo _locationRepo;
        public LocationController(DbContextOptions<AquaContext> context)
        {
            _locationRepo = new LocationRepo(context);
        }

        // GET: Location
        public IActionResult Index()
        {
            var result = _locationRepo.GetAllLocations();
            return View(result);
        }

        // GET: Location/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationEntity = _locationRepo.GetLocationById(id);
            if (locationEntity == null)
            {
                return NotFound();
            }

            return View(locationEntity);
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

        // GET: Location/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationEntity = _locationRepo.GetLocationById(id);
            if (locationEntity == null)
            {
                return NotFound();
            }
            return View(locationEntity);
        }

        // POST: Location/Edit/5
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

        // // // GET: Location/Delete/5
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

        // POST: Location/Delete/5
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
            bool exist = _locationRepo.GetLocationById(id) != null;
            return exist;
        }
    }
}
