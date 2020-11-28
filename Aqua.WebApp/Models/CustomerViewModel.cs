using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Aqua.Library;

namespace Aqua.WebApp.Models
{
    public class CustomerViewModel
    {
        public CustomerViewModel(Customer customer) {
            Id = customer.Id;
            LastName = customer.LastName;
            FirstName = customer.FirstName;
            Email = customer.Email;
        }
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(2048)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [MaxLength(2048)]
        public string FirstName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(2048)]
        public string Email { get; set; }
    }
}
