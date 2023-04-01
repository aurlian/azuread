using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;

namespace ServiceApp.Pages
{
    public class TokenModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public TokenModel(IConfiguration configuration)
        {
            _configuration = configuration;
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

            ViewData["accessToken"] = authResult.AccessToken;
        }
    }
}
