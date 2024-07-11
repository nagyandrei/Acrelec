using Acrelec.SCO.Core.Model.RestExchangedMessages;
using Microsoft.AspNetCore.Mvc;

namespace Acrelec.SCO.Server.Controllers
{
    [ApiController]
    [Route("api-sco/v1/availability")]
    public class AvailabilityController : ControllerBase
    {
        [HttpGet]
        public ActionResult<CheckAvailabilityResponse> GetAvailability()
        {
            return Ok(new CheckAvailabilityResponse { CanInjectOrders = true });
        }
    }
}
