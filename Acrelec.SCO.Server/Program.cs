using Acrelec.SCO.Core.Model.RestExchangedMessages;
using Acrelec.SCO.Server.Interfaces;
using Acrelec.SCO.Server.RequestHandlers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Acrelec.SCO.Server
{
    class Program
    {
        private static readonly CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            Console.WriteLine("This is the SCO Server");

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/api-sco/v1/");
            listener.Start();
            Console.WriteLine("Listening...");

            var inputTask = Task.Run(() =>
            {
                while (true)
                {
                    var input = Console.ReadLine();
                    if (input?.ToLower() == "exit")
                    {
                        cts.Cancel();
                        break;
                    }
                }
            });

            var listenTask = ListenForRequestsAsync(listener, cts.Token);

            // Wait for the input task to complete
            await inputTask;

            // Stop the listener and wait for the listening task to complete
            listener.Stop();
            await listenTask;

            listener.Close();
            Console.WriteLine("Server terminated safely.");
            Console.ReadLine();
        }

        private static async Task ListenForRequestsAsync(HttpListener listener, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var context = await listener.GetContextAsync();
                    await ProcessRequest(context);
                }
            }
            catch (HttpListenerException) when (token.IsCancellationRequested)
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing request: {ex.Message}");
            }
        }

        private static async Task ProcessRequest(HttpListenerContext context)
        {
            Console.WriteLine($"Received new request...");

            var request = context.Request;
            var response = context.Response;
            IRequestHandler handler = null;

            switch (request.HttpMethod)
            {
                case "GET":
                    if (request.Url.AbsolutePath == "/api-sco/v1/availability")
                    {
                        handler = new AvailabilityRequestHandler();
                    }
                    break;
                case "POST":
                    if (request.Url.AbsolutePath == "/api-sco/v1/injectorder")
                    {
                        handler = new InjectOrderRequestHandler();
                    }
                    break;
            }

            if (handler != null)
            {
                await handler.HandleRequestAsync(request, response);
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Close();
            }

            Console.WriteLine("Request successfully processed.");
        }

        private static async Task HandleAvailability(HttpListenerResponse response)
        {
            var responseString = JsonConvert.SerializeObject(new { CanInjectOrders = true });
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.ContentType = "application/json";
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }

        private static async Task HandleInjectOrder(HttpListenerRequest request, HttpListenerResponse response)
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
