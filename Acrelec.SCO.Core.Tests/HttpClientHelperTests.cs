using Acrelec.SCO.Core.Helpers;
using Acrelec.SCO.Core.Interfaces;
using Acrelec.SCO.Core.Managers;
using Acrelec.SCO.Core.Model.RestExchangedMessages;
using Acrelec.SCO.Core.Providers;
using Acrelec.SCO.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Acrelec.SCO.Core.Tests
{
    [TestClass]
    public class HttpClientHelperTests
    {
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private HttpClient httpClient;

        [TestInitialize]
        public void TestInitialize()
        {
            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            httpClient = new HttpClient(mockHttpMessageHandler.Object);
            HttpClientHelper.HttpClient = httpClient; 
        }

        [TestMethod]
        public async Task InjectOrderAsync_Success_ReturnsOrderNumber()
        {
            var order = CreateNewOrder();
            var expectedOrderNumber = "10";
            var injectOrderResponse = new InjectOrderResponse { OrderNumber = expectedOrderNumber };
            var jsonResponse = JsonConvert.SerializeObject(injectOrderResponse);

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var orderManager = new OrderManager(new ItemsProvider());

            var result = await orderManager.InjectOrderAsync(order);

            Assert.AreEqual(expectedOrderNumber, result);
        }

        [TestMethod]
        public async Task HttpGet_Success_ReturnsResponseContent()
        {
            var expectedResponse = "Response content";
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedResponse)
                });

            var result = await HttpClientHelper.HttpGet("http://localhost");

            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public async Task HttpPost_Success_ReturnsResponseContent()
        {
            var expectedResponse = "Response content";
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedResponse)
                });

            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            var result = await HttpClientHelper.HttpPost("http://localhost", content);

            Assert.AreEqual(expectedResponse, result);
        }

        [TestMethod]
        public async Task HttpPost_Throws_Exception_When_Error_Occurs()
        {
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post), ItExpr.IsAny<CancellationToken>())
                .Throws(new HttpRequestException());

            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            var exception = await Assert.ThrowsExceptionAsync<HttpRequestException>(async () =>
            {
                await HttpClientHelper.HttpPost("http://localhost", content);
            });
        }

        [TestMethod]
        public async Task HttpGet_Throws_Exception_When_Error_Occurs()
        {
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get), ItExpr.IsAny<CancellationToken>())
                .Throws(new HttpRequestException());

            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            var exception = await Assert.ThrowsExceptionAsync<HttpRequestException>(async () =>
            {
                await HttpClientHelper.HttpGet("http://localhost");
            });
        }

        private static Order CreateNewOrder()
        {
            var newOrder = new Order();
            newOrder.OrderItems.Add(new OrderedItem { ItemCode = "100", Qty = 1 });
            newOrder.OrderItems.Add(new OrderedItem { ItemCode = "200", Qty = 2 });
            return newOrder;
        }
    }
}
