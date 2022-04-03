using Microsoft.AspNetCore.Mvc;

namespace pProxy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        [HttpGet("/test")]
        public string Test()
        {
            return "test";
        }


        [HttpPost("/auth")]
        public async Task Auth()
        {
            using (var streamContent = new StreamContent(Request.Body))
            {

                var _httpClient = new HttpClient();
                var response = await _httpClient.PostAsync("https://form.hcresort.ru/api/auth", streamContent);
                var content = await response.Content.ReadAsStringAsync();

                Response.StatusCode = (int)response.StatusCode;

                Response.ContentType = response.Content.Headers.ContentType?.ToString();
                Response.ContentLength = response.Content.Headers.ContentLength;

                await Response.WriteAsync(content);
            }
        }

        [HttpGet("/items")]
        public async Task Items()
        {
            var _httpClient = new HttpClient();

            using (var request = new HttpRequestMessage(HttpMethod.Get, "https://form.hcresort.ru/api/items"))
            {

                var header = Request.Headers.FirstOrDefault(h => h.Key.ToLowerInvariant() == "Auth".ToLowerInvariant()).Value.ToString();
                if (header != null)
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