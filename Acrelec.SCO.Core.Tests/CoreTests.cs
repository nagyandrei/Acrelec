using Acrelec.SCO.Core.Interfaces;
using Acrelec.SCO.Core.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Acrelec.SCO.Core.Tests
{
    [TestClass]
    public class CoreTests
    {
        [TestMethod]
        public void ItemsProviderTest()
        {
            IItemsProvider itemsProvider = new ItemsProvider();
            Assert.AreEqual(4, itemsProvider.AllPOSItems.Count, "Different number of items are expected");
            Assert.AreEqual(3, itemsProvider.AvailablePOSItems.Count, "Different number of items are expected");
            //todo - write an assert to check only for items that are available (IsAvailable=True)
        }

        [TestMethod]
        public void OrderedItemsByCodeTest()
        {
            IItemsProvider itemsProvider = new ItemsProvider();
            string[] expectedCodesOrder = new[] { "200", "100", "101", "50" };

            var orderedItems = itemsProvider.AllPOSItems.OrderByDescending(_ => Convert.ToInt32(_.ItemCode)).ToArray();
            string[] orderedCodes = orderedItems.Select(_ => _.ItemCode).ToArray();

            //compare the ordered itemCodes to see it matches the expected order
            Assert.AreEqual(expectedCodesOrder, orderedCodes);
        }
    }
}
