using Aqua.Library;
using System.ComponentModel.DataAnnotations;

namespace Aqua.WebApp.Models
{
    public class CustomerViewModel
    {
        public CustomerViewModel(Customer customer)
        {
            Id = customer.Id;
            LastName = customer.LastName;
            FirstName = customer.FirstName;
            Email = customer.Email;
        }
        public int Id { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        [MaxLength(2048)]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        [MaxLength(2048)]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(2048)]
        public string Email { get; set; }
    }
}
