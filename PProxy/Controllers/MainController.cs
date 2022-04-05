using Microsoft.AspNetCore.Mvc;

namespace pProxy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {

        private const string PAPI_URL_NAME = "PAPI_URL";

        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        public MainController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            var env = Environment.GetEnvironmentVariable(PAPI_URL_NAME);
            _baseUrl = env !=null? env : "https://form.hcresort.ru/api";
        }

        [HttpGet("/config")]
        public string Config()
        {
            return "config: "+ _baseUrl;
        }

        [HttpGet("/api/{**catchAll}")]
        public async Task CatchAllGet(string catchAll)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, _baseUrl + "/" + catchAll))
            {
                var header = Request.Headers.FirstOrDefault(h => h.Key.ToLowerInvariant() == "Auth".ToLowerInvariant()).Value.ToString();
                if (!string.IsNullOrEmpty(header))
                {
                    request.Headers.Add("Auth", header);
                }

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                Response.StatusCode = (int)response.StatusCode;
                Response.ContentType = response.Content.Headers.ContentType.ToString();
                Response.ContentLength = response.Content.Headers.ContentLength;

                await Response.WriteAsync(content);
            }
        }

        [HttpPost("/api/{**catchAll}")]
        public async Task CatchAllPost(string catchAll)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/" + catchAll))
            {
                using (var streamContent = new StreamContent(Request.Body))
                {
                    request.Content = streamContent;

                    var header = Request.Headers.FirstOrDefault(h => h.Key.ToLowerInvariant() == "Auth".ToLowerInvariant()).Value.ToString();
                    if (!string.IsNullOrEmpty(header))
                    {
                        request.Headers.Add("Auth", header);
                    }

                    var response = await _httpClient.SendAsync(request);
                    var content = await response.Content.ReadAsStringAsync();

                    Response.StatusCode = (int)response.StatusCode;
                    Response.ContentType = response.Content.Headers.ContentType.ToString();
                    Response.ContentLength = response.Content.Headers.ContentLength;

                    await Response.WriteAsync(content);
                }
            }
        }

    }
}