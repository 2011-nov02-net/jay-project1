using Aqua.Library;
using System.Collections.Generic;

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
