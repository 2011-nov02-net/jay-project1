using System.Collections.Generic;

namespace Aqua.Data.Model
{
    public class CustomerEntity
    {
        public CustomerEntity()
        {
            Orders = new HashSet<OrderEntity>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; }
    }
}
