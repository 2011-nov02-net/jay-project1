namespace Aqua.Data.Model
{
    public class InventoryItemEntity
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int AnimalId { get; set; }
        public int Quantity { get; set; }

        public virtual AnimalEntity Animal { get; set; }
        public virtual LocationEntity Location { get; set; }
    }
}