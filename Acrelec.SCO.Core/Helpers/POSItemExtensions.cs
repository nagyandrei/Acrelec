using Acrelec.SCO.DataStructures;
using System;

namespace Acrelec.SCO.Core.Helpers
{
    public static class POSItemExtensions
    {
        public static string Name(POSItem item)
        {
            if (item == null || string.IsNullOrEmpty(item.Name))
                throw new ArgumentNullException("Item or Item.Name is null!");

            return item.Name.Length <= 3 ? item.Name : item.Name.Substring(0, 3);
        }
    }
}
