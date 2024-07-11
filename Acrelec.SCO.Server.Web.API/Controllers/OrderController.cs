using Acrelec.SCO.Core.Model.RestExchangedMessages;
using Microsoft.AspNetCore.Mvc;

namespace Acrelec.SCO.Server.Controllers
{
    [ApiController]
    [Route("api-sco/v1")]
    public class OrderController : ControllerBase
    {
        [HttpPost("injectorder")]
        public ActionResult<InjectOrderResponse> InjectOrder([FromBody] InjectOrderRequest request)
        {
            if (request.Customer == null || string.IsNullOrWhiteSpace(request.Customer.Firstname) || string.IsNullOrWhiteSpace(request.Customer.Address))
            {
                return BadRequest("Customer details are missing!");
            }

            return Ok(new InjectOrderResponse { OrderNumber = "10" });
        }
    }
}
