using System;
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
        public OrderController(DbContextOptions<AquaContext> context)
        {
            _customerRepo = new CustomerRepo(context);
            _locationRepo = new LocationRepo(context);
            _orderRepo = new OrderRepo(context);
        }

        // GET: OrderController
        public ActionResult Index()
        {
            List<Order> result = _orderRepo.GetAllOrders();
            return View(result);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Order order)
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

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
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

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Order order)
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
    }
}
