using System;

namespace Supermarket
{
    public delegate double ItemOffer(Item item, double quantity);

    public class Offer
    {
        public Offer(int id, ItemOffer itemOffer)
        {
            Id = id;
            ItemOffer = itemOffer;
        }

        public int Id { get; set; }

        public ItemOffer ItemOffer { get; set; }

        public static ItemOffer BuyXGetYFree(int x, int y)
        {
            return delegate(Item item, double quantity)
            {
                if (item.ItemType.Equals(ItemType.Weighted))
                {
                    throw  new Exception("Weighted item is not applicable for this offer");
                }
                int discount = (int) (quantity/(x + y))*y;
                Console.WriteLine("Buy {0} get {1} free, initial:{2} -> endQuantity:{3}", x, y, quantity,
                    quantity - discount);
                return item.Price*(quantity - discount);
            };
        }

        public static ItemOffer BuyXatYPrice(int x, double y)
        {
            return delegate(Item item, double quantity)
            {
                if (item.ItemType.Equals(ItemType.Weighted))
                {
                    throw new Exception("Weighted item is not applicable for this offer");
                }
                int specialPrice = (int) (quantity/x);
                int newQuantity = (int) (quantity - specialPrice*x);
                Console.WriteLine("Buy {0} at {1} price, initial:{2} -> endQuantity:{3}", x, y, quantity, newQuantity);
                return item.Price*newQuantity + specialPrice*y;
            };
        }

        public static ItemOffer DiscountPercent(double percentage)
        {
            return delegate(Item item, double quantity) { return item.Price*quantity*percentage/100.0; };
        }

        public static ItemOffer NoOffer()
        {
            return delegate(Item item, double quantity) { return item.Price*quantity; };
        }
    }
}