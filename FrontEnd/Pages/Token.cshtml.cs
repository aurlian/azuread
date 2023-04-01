using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;

namespace FrontEnd.Pages
{
    public class TokenModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenAcquisition _tokenAcquisition;

        public TokenModel(ILogger<IndexModel> logger, IConfiguration configuration, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _configuration = configuration;
            _tokenAcquisition = tokenAcquisition;
        }
        public async Task OnGet()
        {
            string[] scopes = new string[] { _configuration["AzureAd:UserScope"] };
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

            ViewData["accessToken"] = accessToken;
        }
    }
}
