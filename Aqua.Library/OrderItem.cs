namespace Aqua.Library
{
    public class OrderItem
    {
        public OrderItem(int orderId, Animal animal, int quantity, decimal total)
        {
            OrderId = orderId;
            Animal = animal;
            Quantity = quantity;
            Total = total;
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Animal Animal { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
