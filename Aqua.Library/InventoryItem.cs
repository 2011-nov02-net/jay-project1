namespace Aqua.Library
{
    public class InventoryItem
    {
        public InventoryItem()
        {
        }
        public InventoryItem(int locationId, string animalName, int quantity)
        {
            LocationId = locationId;
            AnimalName = animalName;
            Quantity = quantity;
        }
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string AnimalName { get; set; }
        public int Quantity { get; set; }
    }
}