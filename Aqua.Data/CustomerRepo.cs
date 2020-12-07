using Aqua.Data.Model;
using Aqua.Library;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Aqua.Data
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly DbContextOptions<AquaContext> _contextOptions;
        public CustomerRepo(DbContextOptions<AquaContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        public List<Customer> GetAllCustomers()
        {
            using AquaContext context = new AquaContext(_contextOptions);
            List<CustomerEntity> dbCust = context.Customers.Distinct().ToList();
            List<Customer> result = new List<Customer>();
            foreach (CustomerEntity customer in dbCust)
            {
                Customer newCustomer = new Customer()
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email
                };
                result.Add(newCustomer);
            };
            return result;
        }
        public Customer GetCustomerByEmail(string email)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            CustomerEntity dbCust = context.Customers
                .Where(a => a.Email == email)
                .FirstOrDefault();
            if (dbCust != null)
            {
                Customer newCust = new Customer()
                {
                    Id = dbCust.Id,
                    FirstName = dbCust.FirstName,
                    LastName = dbCust.LastName,
                    Email = dbCust.Email
                };
                return newCust;
            }
            else
            {
                return null;
            }
        }
        public List<Customer> GetCustomerByName(string first, string last)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            List<CustomerEntity> dbCust = context.Customers
                .Where(c => c.FirstName == first)
                .Where(c => c.LastName == last)
                .ToList();
            List<Customer> allCust = new List<Customer>();
            foreach (CustomerEntity cust in dbCust)
            {
                allCust.Add(GetCustomerById(cust.Id));
            }
            return allCust;
        }
        public Customer GetCustomerById(int? id)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            CustomerEntity dbCust = context.Customers
                .Where(c => c.Id == id)
                .FirstOrDefault();
            if (dbCust == null)
            {
                return null;
            }
            Customer newCust = new Customer()
            {
                Id = dbCust.Id,
                LastName = dbCust.LastName,
                FirstName = dbCust.FirstName,
                Email = dbCust.Email
            };
            return newCust;
        }
        public void CreateCustomerEntity(Customer customer)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            CustomerEntity newEntry = new CustomerEntity()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            };
            context.Customers.Add(newEntry);
            context.SaveChanges();
        }
        public void UpdateCustomerEntity(Customer customer)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            CustomerEntity dbCust = context.Customers
                .Where(a => a.Id == customer.Id)
                .FirstOrDefault();
            dbCust.FirstName = customer.FirstName;
            dbCust.LastName = customer.LastName;
            dbCust.Email = customer.Email;
            try
            {
                context.SaveChanges();
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
        }
        public void DeleteCustomerEntity(Customer customer)
        {
            using AquaContext context = new AquaContext(_contextOptions);
            CustomerEntity dbCustomer = context.Customers
                .Where(i => i.Id == customer.Id)
                .FirstOrDefault();
            context.Remove(dbCustomer);
            context.SaveChanges();
        }
    }
}