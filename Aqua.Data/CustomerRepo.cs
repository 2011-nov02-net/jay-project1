﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Aqua.Data.Model;
using Aqua.Library;


namespace Aqua.Data
{
    public class CustomerRepo
    {
        private readonly DbContextOptions<AquaContext> _contextOptions;
        public CustomerRepo(DbContextOptions<AquaContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }
        public ICollection<CustomerEntity> GetAllCustomers()
        {
            using var context = new AquaContext(_contextOptions);
            var dbCust = context.Customers.Distinct().ToList();
            return dbCust;
        }
        public Customer GetCustomerByEmail(string email)
        {
            using var context = new AquaContext(_contextOptions);
            var dbCust = context.Customers
                .Where(a => a.Email == email)
                .FirstOrDefault();
            var newCust = new Customer()
            {
                Id = dbCust.Id,
                FirstName = dbCust.FirstName,
                LastName = dbCust.LastName,
                Email = dbCust.Email
            };
            return newCust;
        }
        public ICollection<Customer> GetCustomerByName(string first, string last)
        {
            using var context = new AquaContext(_contextOptions);
            var dbCust = context.Customers
                .Where(c => c.FirstName == first)
                .Where(c => c.LastName == last)
                .ToList();
            var allCust = new List<Customer>();
            foreach (var cust in dbCust)
            {
                allCust.Add(GetCustomerById(cust.Id));
            }
            return allCust;
        }
        public Customer GetCustomerById(int id)
        {
            using var context = new AquaContext(_contextOptions);
            var dbCust = context.Customers
                .Where(c => c.Id == id)
                .FirstOrDefault();
            var newCust = new Customer()
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
            using var context = new AquaContext(_contextOptions);
            var newEntry = new CustomerEntity()
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
            using var context = new AquaContext(_contextOptions);
            var dbCust = context.Customers
                .Where(a => a.Id == customer.Id)
                .FirstOrDefault();
            dbCust.FirstName = customer.FirstName;
            dbCust.LastName = customer.LastName;
            dbCust.Email = customer.Email;
            context.SaveChanges();
        }
    }
}