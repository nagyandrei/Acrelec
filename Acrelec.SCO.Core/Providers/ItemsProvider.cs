using Acrelec.SCO.Core.Interfaces;
using Acrelec.SCO.DataStructures;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Acrelec.SCO.Core.Providers
{
    /// <summary>
    /// class providing the list of items as retrieved from POS system
    /// </summary>
    public class ItemsProvider : IItemsProvider
    {
        private List<POSItem> _posItems;

        public List<POSItem> AllPOSItems => _posItems;

        List<POSItem> IItemsProvider.AvailablePOSItems => _posItems.FindAll(_ => _.IsAvailable);

        /// <summary>
        /// constructor
        /// </summary>
        public ItemsProvider()
        {
            _posItems = new List<POSItem>();
            LoadItemsFromPOS();
        }

        /// <summary>
        /// retrieving items from POS is a simple parse of a json
        /// </summary>
        public void LoadItemsFromPOS()
        {
            var json = File.ReadAllText("Data/ContentItems.json");
            _posItems = JsonConvert.DeserializeObject<List<POSItem>>(json);
        }
    }
}
