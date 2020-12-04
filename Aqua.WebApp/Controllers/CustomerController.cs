using Aqua.Data;
using Aqua.Library;
using Aqua.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Aqua.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ILogger<CustomerController> logger, ICustomerRepo customerRepo, IOrderRepo orderRepo)
        {
            _logger = logger;
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
        }
        // GET: Customer
        public IActionResult Index(string searchString)
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
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return Error();
            }
            var customer = _customerRepo.GetCustomerById(id);
            if (customer == null)
            {
                return Error();
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
        public IActionResult Create()
        {
            if (!ModelState.IsValid)
            {
                return Error();
            }
            if (TempData["EmailExistsError"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["EmailExistsError"].ToString());
            }
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (CustomerEmailExists(customer.Email))
            {
                TempData["EmailExistsError"] = $"Email address '{customer.Email}' already exists in our database.";
                return RedirectToAction("Create");
            }
            else
            {
                _customerRepo.CreateCustomerEntity(customer);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Customer/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Error();
            }
            if (TempData["EmailExistsError"] != null)
            {
                ModelState.AddModelError(string.Empty, TempData["EmailExistsError"].ToString());
            }
            var customer = _customerRepo.GetCustomerById(id);
            if (customer == null)
            {
                return Error();
            }
            var result = new CustomerViewModel(customer);
            return View(result);

        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return Error();
            }
            if (CustomerEmailExists(customer.Email) && !EmailSameDuringEditing(id, customer.Email))
            {
                TempData["EmailExistsError"] = $"Email address '{customer.Email}' already exists in our database.";
                return RedirectToAction("Edit", new { Id = customer.Id });
            }
            else if (ModelState.IsValid)
            {
                try
                {
                    _customerRepo.UpdateCustomerEntity(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (CustomerIdExists(id))
                    {
                        return Error();
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
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Error();
            }
            var customer = _customerRepo.GetCustomerById(id);
            if (customer == null)
            {
                return Error();
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
        private bool EmailSameDuringEditing(int id, string email)
        {
            var currentCustomer = _customerRepo.GetCustomerById(id);
            var changingEmail = _customerRepo.GetCustomerByEmail(email);
            bool samePersonCheck = (currentCustomer.Id == changingEmail.Id);
            return samePersonCheck;
        }
        public IActionResult Error()
        {
            _logger.LogError($"Error in customer controller");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
