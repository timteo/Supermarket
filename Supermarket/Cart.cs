using System.Collections.Generic;

namespace Supermarket
{
    public class Cart
    {
        public Cart()
        {
            ItemList = new Dictionary<Item, double>();
        }

        public Dictionary<Item, double> ItemList { get; set; }

        public void AddItem(Item item, int value)
        {
            double curVal;

            if (ItemList.TryGetValue(item, out curVal))
            {
                ItemList[item] += value;
            }
            else
            {
                ItemList.Add(item, value);
            }
        }
    }
}