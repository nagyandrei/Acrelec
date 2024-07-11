using Acrelec.SCO.Core.Helpers;
using Acrelec.SCO.Core.Interfaces;
using Acrelec.SCO.Core.Model.RestExchangedMessages;
using Acrelec.SCO.DataStructures;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Acrelec.SCO.Core.Managers
{
    public class OrderManager : IOrderManager
    {
        private IItemsProvider _itemsProvider { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public OrderManager(IItemsProvider itemsProvider)
        {
            _itemsProvider = itemsProvider;
        }

        public async Task<string> InjectOrderAsync(Order orderToInject)
        {
            var injectOrderRequest = new InjectOrderRequest { Order = orderToInject, Customer = new Customer { Address = "Bucharest", Firstname = "John" } };
            var jsonContent = JsonConvert.SerializeObject(injectOrderRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var result = await HttpClientHelper.HttpPost("http://localhost:5000/api-sco/v1/injectorder", content);

            return JsonConvert.DeserializeObject<InjectOrderResponse>(result).OrderNumber;
        }
    }
}
