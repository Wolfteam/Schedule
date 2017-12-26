using Microsoft.IdentityModel.Tokens;

namespace Schedule.API.Helpers
{    
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/token";

        public string Issuer { get; set; }

        public string[] Audience { get; set; }

        public SigningCredentials SigningCredentials { get; set; }
    }
}