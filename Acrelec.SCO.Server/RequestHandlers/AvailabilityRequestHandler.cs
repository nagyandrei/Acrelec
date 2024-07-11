using Acrelec.SCO.Server.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Acrelec.SCO.Server.RequestHandlers
{
    public class AvailabilityRequestHandler : IRequestHandler
    {
        public async Task HandleRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var responseString = JsonConvert.SerializeObject(new { CanInjectOrders = true });
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.ContentType = "application/json";
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }
    }
}
