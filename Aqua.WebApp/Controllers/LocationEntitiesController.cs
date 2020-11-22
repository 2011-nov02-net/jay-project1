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
    public class LocationEntitiesController : Controller
    {
        private LocationRepo _locationRepo;
        public LocationEntitiesController(DbContextOptions<AquaContext> context)
        {
            _locationRepo = new LocationRepo(context);
        }

        // GET: LocationEntities
        public IActionResult Index()
        {
            var result = _locationRepo.GetAllLocations();
            return View(result);
        }

        // GET: LocationEntities/Details/5
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

        // GET: LocationEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocationEntities/Create
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

        // GET: LocationEntities/Edit/5
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

        // POST: LocationEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    if (LocationEntityExists(id))
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

        // // GET: LocationEntities/Delete/5
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

        // // POST: LocationEntities/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public IActionResult DeleteConfirmed(int id)
        // {
        //     var location = _locationRepo.GetLocationById(id);
        //     _context.Locations.Remove(locationEntity);
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        private bool LocationEntityExists(int id)
        {
            bool exist = _locationRepo.GetLocationById(id) != null;
            return exist;
        }
    }
}
