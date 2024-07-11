using Acrelec.SCO.Core.Helpers;
using Acrelec.SCO.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Acrelec.SCO.Core.Tests
{
    [TestClass]
    public class POSItemExtensionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Name_Throws_Exception_If_POSItem_Is_Null()
        {
            POSItemExtensions.Name(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Name_Throws_Exception_If_POSItem_Name_Property_Is_Empty()
        {
            var emptyName = new POSItem { Name = string.Empty };

            POSItemExtensions.Name(emptyName);
        }

        [TestMethod]
        public void Name_With_Lenght_Less_Than_3_Characters_Return_Name_Property()
        {
            var name1 = new POSItem { Name = "Tea" };
            var name2 = new POSItem { Name = "AB" };

            var result1 = POSItemExtensions.Name(name1);
            var result2 = POSItemExtensions.Name(name2);

            Assert.AreEqual("Tea", result1);
            Assert.AreEqual("AB", result2);
        }

        [TestMethod]
        public void Name_With_Greater_Than_3_Characters_Returns_First_3_Characters()
        {
            var name = new POSItem { Name = "CocaCola" };

            var result = POSItemExtensions.Name(name);

            Assert.AreEqual("Coc", result);
        }
    }
}