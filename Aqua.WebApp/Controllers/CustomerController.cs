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
        private ICustomerRepo _customerRepo;
        private IOrderRepo _orderRepo;
        public CustomerController(ICustomerRepo customerRepo, IOrderRepo orderRepo)
        {
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
        }
        // GET: Customer
        public ActionResult Index(string searchString)
        {
            List<Customer> customerList = _customerRepo.GetAllCustomers();
            var result = new List<CustomerViewModel>();
            foreach (var cust in customerList)
            {
                var newCust = new CustomerViewModel(cust);
                result.Add(newCust);
            }
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
            var result = new CustomerViewModel(customer);
            return View(result);
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
            if (TempData["EmailExistsError"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["EmailExistsError"].ToString());
            }
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (CustomerEmailExists(customer.Email))
            {
                TempData["EmailExistsError"] = "Email already exists in our database.";
                return RedirectToAction("Create");
            }
            else
            {
                _customerRepo.CreateCustomerEntity(customer);
                return RedirectToAction(nameof(Index));
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
                    if (CustomerIdExists(id))
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
            var result = new CustomerViewModel(customer);
            return View(result);
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
        private bool CustomerIdExists(int id)
        {
            bool exist = (_customerRepo.GetCustomerById(id) != null);
            return exist;
        }
        private bool CustomerEmailExists(string email)
        {
            bool exist = (_customerRepo.GetCustomerByEmail(email) != null); // If it returns not null it exists, so true
            return exist;
        }
    }
}
