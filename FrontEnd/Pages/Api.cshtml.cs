using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;

namespace FrontEnd.Pages
{
    public class ApiModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly HttpClient _httpClient;

        public ApiModel(ILogger<IndexModel> logger, IConfiguration configuration,
            ITokenAcquisition tokenAcquisition, HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _tokenAcquisition = tokenAcquisition;
            _httpClient = httpClient;
        }
        public async Task OnGet()
        {
            string[] scopes = new string[] { _configuration["AzureAd:UserScope"] };
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.GetAsync($"{_configuration["ApiUrl"]}/WeatherForecast");

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                ViewData["content"] = content;
            }

            ViewData["statusCode"] = response.StatusCode;
            ViewData["accessToken"] = accessToken;
        }
    }
}
