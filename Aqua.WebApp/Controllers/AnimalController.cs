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
using System.Diagnostics;

namespace Aqua.WebApp.Controllers
{
    public class AnimalController : Controller
    {
        private IAnimalRepo _animalRepo;
        public AnimalController(IAnimalRepo animalRepo)
        {
            _animalRepo = animalRepo;
        }
        // GET: Animal
        public IActionResult Index()
        {
            var animals = _animalRepo.GetAllAnimals();
            var result = new List<AnimalViewModel>();
            foreach(var animal in animals)
            {
                var newAnimal = new AnimalViewModel()
                {
                    Id = animal.Id,
                    Name = animal.Name,
                    Price = animal.Price
                };
                result.Add(newAnimal);
            }
            return View(result);
        }

        // GET: Animal/Details/5
        public IActionResult Details()
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
            return View();
        }

        // GET: Animal/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Animal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Animal animal)
        {
            try
            {
                _animalRepo.CreateAnimalEntity(animal);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Animal/Edit/5
        public IActionResult Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
            return View();
        }

        // POST: Animal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Animal animal)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return Error();
            }
        }

        // GET: Animal/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Animal/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Animal animal)
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
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
