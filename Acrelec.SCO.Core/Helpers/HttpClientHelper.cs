using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Acrelec.SCO.Core.Helpers
{
    public static class HttpClientHelper
    {
        public static HttpClient HttpClient { get; set; } = new HttpClient();

        public static async Task<string> HttpGet(string url)
        {
            try
            {
                var response = await HttpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HttpGet error: {ex.Message}");
                throw;
            }
        }

        public static async Task<string> HttpPost(string url, HttpContent content)
        {
            try
            {
                var response = await HttpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HttpPost error: {ex.Message}");
                throw;
            }
        }
    }
}
