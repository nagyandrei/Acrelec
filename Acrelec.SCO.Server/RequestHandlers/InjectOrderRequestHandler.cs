using Acrelec.SCO.Core.Model.RestExchangedMessages;
using Acrelec.SCO.Server.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Acrelec.SCO.Server.RequestHandlers
{
    public class InjectOrderRequestHandler : IRequestHandler
    {
        public async Task HandleRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                var requestBody = await reader.ReadToEndAsync();
                var injectOrderRequest = JsonConvert.DeserializeObject<InjectOrderRequest>(requestBody);

                if (injectOrderRequest.Customer == null || string.IsNullOrWhiteSpace(injectOrderRequest.Customer.Firstname) || string.IsNullOrWhiteSpace(injectOrderRequest.Customer.Address))
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusDescription = "Customer details are missing!";
                }
                else
                {
                    var responseString = JsonConvert.SerializeObject(new { OrderNumber = "10" });
                    var buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "application/json";
                    await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            response.Close();
        }
    }
}
