using System;
using System.Collections.Generic;
using System.Text;
using Aqua.Library;

namespace Aqua.Data
{
    public interface ICustomerRepo
    {
        List<Customer> GetAllCustomers();
        Customer GetCustomerByEmail(string email);
        List<Customer> GetCustomerByName(string first, string last);
        Customer GetCustomerById(int? id);
        void CreateCustomerEntity(Customer customer);
        void UpdateCustomerEntity(Customer customer);
        void DeleteCustomerEntity(Customer customer);
    }
}
