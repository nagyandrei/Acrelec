using Acrelec.SCO.DataStructures;

namespace Acrelec.SCO.Core.Model.RestExchangedMessages
{
    public class InjectOrderRequest
    {
        public Order Order { get; set; }
        public Customer Customer { get; set; }
    }
}
