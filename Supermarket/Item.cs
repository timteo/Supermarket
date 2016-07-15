namespace Supermarket
{
    public class Item
    {
        private static int _runningId = 1;

        public Item(string name, double cost, double price, ItemType itemType)
        {
            Id = _runningId++;
            Name = name;
            Cost = cost;
            Price = price;
            ItemType = itemType;
            Offer = Supermarket.Offer.NoOffer();
        }


        public int Id { get; private set; }

        public string Name { get; set; }

        public ItemType ItemType { get; set; }

        public double Cost { get; set; }

        public double Price { get; set; }

        public ItemOffer Offer { get; set; }

        public double CalculatelPrice(double quantity)
        {
            return Offer.Invoke(this, quantity);
        }
    }
}