using System;
using System.ComponentModel.DataAnnotations;

namespace Aqua.WebApp.Models
{
    public class AnimalViewModel
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]
        [Range(0, 100000)]
        public double Price { get; set; }
    }
}
