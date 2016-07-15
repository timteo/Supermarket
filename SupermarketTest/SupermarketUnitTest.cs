using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Supermarket;

namespace SupermarketTest
{
    [TestFixture]
    public class SupermarketUnitTest
    {
        private static SupermarketSystem _supermarketSystem;
        private static Cart _cart;

        public SupermarketUnitTest()
        {
            Setup();
        }

        public static IEnumerable<TestCaseData> GetCartTestData()
        {
            Setup(); //to prevent null exception since static methods are initialized first
            //Item,quantity, expected
            yield return new TestCaseData(_supermarketSystem.ItemList[0], 5, 5);
            yield return new TestCaseData(_supermarketSystem.ItemList[0], 3, 8);
            yield return new TestCaseData(_supermarketSystem.ItemList[1], 2, 2);
            yield return new TestCaseData(_supermarketSystem.ItemList[1], 1, 3);
        }


        public static IEnumerable<TestCaseData> GetOfferTestData()
        {
            Setup(); //to prevent null exception since static methods are initialized first
            //Item, OfferMethod(x,y), quantity, expected
            yield return new TestCaseData(_supermarketSystem.ItemList[0], Offer.BuyXGetYFree(2, 1), 3, 4); //price $2
            yield return new TestCaseData(_supermarketSystem.ItemList[0], Offer.BuyXGetYFree(2, 1), 2, 4); //price $2

            yield return new TestCaseData(_supermarketSystem.ItemList[0], Offer.BuyXatYPrice(3, 5), 3, 5); //price $2
            yield return new TestCaseData(_supermarketSystem.ItemList[0], Offer.BuyXatYPrice(3, 5), 5, 9); //price $2

            yield return new TestCaseData(_supermarketSystem.ItemList[0], Offer.NoOffer(), 5, 10); //price $2
        }

        public static void Setup()
        {
            if (_supermarketSystem != null)
            {
                return;
            }

            _supermarketSystem = new SupermarketSystem();

            //Item name, cost, price, item type
            _supermarketSystem.ItemList.Add(new Item("Item A", 1, 2, ItemType.Quantity));
            _supermarketSystem.ItemList.Add(new Item("Item B", 3, 5, ItemType.Quantity));
            _supermarketSystem.ItemList.Add(new Item("Item C", 7, 8, ItemType.Weighted));
            _supermarketSystem.ItemList.Add(new Item("Item D", 8, 11, ItemType.Weighted));

            _supermarketSystem.OfferList.Add(new Offer(1, Offer.BuyXGetYFree(2, 3)));
            _supermarketSystem.OfferList.Add(new Offer(2, Offer.BuyXatYPrice(3, 10)));

            _supermarketSystem.UpdateItemOffers();

            _cart = new Cart();
        }

        [Test]
        public void TestApplyOffer()
        {
            foreach (var i in _supermarketSystem.ItemList)
            {
                var offer = _supermarketSystem.OfferList.SingleOrDefault(o => o.Id == i.Id);
                if (offer == null)
                {
                    Assert.IsTrue(i.Offer == Offer.NoOffer());
                }
                else
                {
                    Assert.IsTrue(i.Offer == offer.ItemOffer);
                }
            }
        }

        [Test]
        [TestCaseSource("GetCartTestData")]
        public void TestCartAdd(Item item, int quantity, int expected)
        {
            _cart.AddItem(item, quantity);
            Assert.AreEqual(expected, _cart.ItemList[item]);
        }

        [Test]
        public void TestCheckOut()
        {
            _cart.ItemList.Clear();

            int item1Quantity = 5;
            int item2Quantity = 5;
            double item1Total = _supermarketSystem.ItemList[0].CalculatelPrice(item1Quantity);
            double item2Total = _supermarketSystem.ItemList[1].CalculatelPrice(item2Quantity);

            _cart.AddItem(_supermarketSystem.ItemList[0], item1Quantity);
            _cart.AddItem(_supermarketSystem.ItemList[1], item2Quantity);

            double expected = item1Total + item2Total;
            double actual = _supermarketSystem.Checkout(_cart);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCaseSource("GetOfferTestData")]
        public void TestOffer(Item item, ItemOffer itemOffer, double quantity, double expected)
        {
            item.Offer = itemOffer;
            double actual = item.CalculatelPrice(quantity);

            Assert.AreEqual(expected, actual);
        }
    }
}