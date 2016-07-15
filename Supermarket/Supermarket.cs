using System.Collections.Generic;
using System.Linq;

namespace Supermarket
{
    public class SupermarketSystem
    {
        public SupermarketSystem()
        {
            OfferList = new List<Offer>();
            ItemList = new List<Item>();
        }

        public List<Offer> OfferList { get; set; }

        public List<Item> ItemList { get; set; }

        public void UpdateItemOffers()
        {
            ItemList.ForEach(i =>
            {
                Offer offer = OfferList.SingleOrDefault(o => o.Id == i.Id);
                if (offer == null)
                {
                    i.Offer = Offer.NoOffer();
                }
                else
                {
                    i.Offer = offer.ItemOffer;
                }
            });
        }

        public double Checkout(Cart cart)
        {
            double total = 0;
            foreach (KeyValuePair<Item, double> pair in cart.ItemList)
            {
                total += pair.Key.CalculatelPrice(pair.Value);
            }
            return total;
        }
    }
}