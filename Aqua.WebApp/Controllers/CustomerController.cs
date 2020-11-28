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
    public class CustomerController : Controller
    {
        private CustomerRepo _customerRepo;
        private OrderRepo _orderRepo;
        public CustomerController(DbContextOptions<AquaContext> context)
        {
            _customerRepo = new CustomerRepo(context);
            _orderRepo = new OrderRepo(context);
        }
        // GET: Customer
        public ActionResult Index(string searchString)
        {
            List<Customer> result = _customerRepo.GetAllCustomers();
            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.FindAll(s => (s.FirstName.Contains(searchString)) || (s.LastName.Contains(searchString)));
            }
            return View(result);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = _customerRepo.GetCustomerById(id);
            if(customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        public IActionResult CustomerOrders(int id)
        {
            var customer = _customerRepo.GetCustomerById(id);
            List<Order> result = _orderRepo.GetOrdersByCustomer(customer);
            return View(result);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            try
            {
                _customerRepo.CreateCustomerEntity(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _customerRepo.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            var result = new CustomerViewModel(customer);
            return View(result);

        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _customerRepo.UpdateCustomerEntity(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (CustomerExists(id))
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
            return View(customer);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = _customerRepo.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var customer = _customerRepo.GetCustomerById(id);
            _customerRepo.DeleteCustomerEntity(customer);
            return RedirectToAction(nameof(Index));
        }
        private bool CustomerExists(int id)
        {
            bool exist = (_customerRepo.GetCustomerById(id) != null);
            return exist;
        }
    }
}
