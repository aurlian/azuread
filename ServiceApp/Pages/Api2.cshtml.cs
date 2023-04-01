using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace ServiceApp.Pages
{
    public class Api2Model : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public Api2Model(ILogger<IndexModel> logger, IConfiguration configuration,
            HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task OnGet()
        {
            IConfidentialClientApplication confidentialApp = ConfidentialClientApplicationBuilder
                .Create(_configuration["AzureAd:ClientId"])
                .WithTenantId(_configuration["AzureAd:TenantId"])
                .WithClientSecret(_configuration["AzureAd:ClientSecret"])
                .Build();

            string[] scopes = new string[] { _configuration["AzureAd:UserScope"] };
            var authResult = await confidentialApp.AcquireTokenForClient(scopes).ExecuteAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _httpClient.PutAsync($"{_configuration["ApiUrl"]}/WeatherForecast?disabled=true", null);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                ViewData["content"] = content;
            }

            ViewData["statusCode"] = response.StatusCode;
        }
    }
}
