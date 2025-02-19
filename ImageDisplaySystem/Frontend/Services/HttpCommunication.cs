using BasicArgs;
using Frontend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend.Services
{
    public class HttpCommunication : IHttpCommunication
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions option;
        public HttpCommunication()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5027/");

            option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }

        public async Task<bool> VerifyAsync(SigninInfoArgs args)
        {
            var content = new StringContent(JsonSerializer.Serialize(args), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/signin/verify", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var temp = JsonSerializer.Deserialize<bool>(result, option);
                return temp;
            }
            return false;
        }

        public async Task<bool> RegisterAsync(SigninInfoArgs args)
        {
            var content = new StringContent(JsonSerializer.Serialize(args), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/signin/register", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var temp = JsonSerializer.Deserialize<bool>(result, option);
                return temp;
            }
            return false;
        }
    }
}
