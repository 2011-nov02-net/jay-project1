using System;
using System.Collections.Generic;

namespace Aqua.Library
{
    public class Order
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public Customer Customer { get; set; }
        public Animal Animal { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
    }
}
