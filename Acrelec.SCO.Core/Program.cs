using Acrelec.SCO.Core.Interfaces;
using Acrelec.SCO.Core.Managers;
using Acrelec.SCO.Core.Providers;
using Acrelec.SCO.DataStructures;
using System;
using System.Threading.Tasks;
using Acrelec.SCO.Core.Helpers;
using Newtonsoft.Json;
using Acrelec.SCO.Core.Model.RestExchangedMessages;

namespace Acrelec.SCO.Core
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("SCO - Self Check Out System");

            //init
            IItemsProvider itemsProvider = new ItemsProvider();
            IOrderManager orderManager = new OrderManager(itemsProvider);

            //list POS items
            ListAllItems(itemsProvider);

            //todo - create an order containing the following items:
            //1*Coke
            //2*Water
            var newOrder = CreateNewOrder();

            if (!await CheckServerAvailability())
            {
                Console.WriteLine("Server is not available for order injection");
                return;
            }

            var assignedOrderNumber = await orderManager.InjectOrderAsync(newOrder);

            if (!string.IsNullOrWhiteSpace(assignedOrderNumber) == true)
                Console.WriteLine($"Order injected with success. OrderNumber: {assignedOrderNumber}");
            else
                Console.WriteLine("Error injecting order");
        }

        private static Order CreateNewOrder()
        {
            var newOrder = new Order();
            newOrder.OrderItems.Add(new OrderedItem { ItemCode = "100", Qty = 1 });
            newOrder.OrderItems.Add(new OrderedItem { ItemCode = "200", Qty = 2 });
            return newOrder;
        }

        private static async Task<bool> CheckServerAvailability()
        {
            var content = await HttpClientHelper.HttpGet("http://localhost:5000/api-sco/v1/availability");
            return JsonConvert.DeserializeObject<CheckAvailabilityResponse>(content).CanInjectOrders;
        }

        /// <summary>
        /// list in Console all items (with all their properties)
        /// </summary>
        private static void ListAllItems(IItemsProvider itemsProvider)
        {
            var items = itemsProvider.AllPOSItems;

            foreach (var item in items)
            {
                Console.WriteLine($"Item name: {item.Name}");
                Console.WriteLine($"Item short name: {POSItemExtensions.Name(item)}");
            }
        }
    }
}
