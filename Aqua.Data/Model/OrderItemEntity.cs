namespace Aqua.Data.Model
{
    public class OrderItemEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int AnimalId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public virtual AnimalEntity Animal { get; set; }
        public virtual OrderEntity Order { get; set; }
    }
}
