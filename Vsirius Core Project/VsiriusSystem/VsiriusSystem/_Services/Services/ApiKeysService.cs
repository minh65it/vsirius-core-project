using Microsoft.Extensions.Options;
using VsiriusSystem._Services.Interfaces;
using VsiriusSystem.Models;

namespace VsiriusSystem._Services.Services
{
    public class ApiKeysService : IApiKeysService
    {
        private readonly Dictionary<string, string> _credentialsDictionary;

        public ApiKeysService(IConfiguration configuration)
        {
            _credentialsDictionary = configuration.GetSection("ApiVsiriusCredentialsSystem").Get<Dictionary<string, string>>();
        }

        public bool ValidateCredentials(string clientId, string clientSecret)
        {
            if (_credentialsDictionary.TryGetValue(clientId, out var expectedSecret))
            {
                return clientSecret == expectedSecret;
            }

            return false;
        }

        public bool ValidateCredentialsBackOffice(string clientSecret)
        {
            return _credentialsDictionary["BackOffice"] == clientSecret;
        }
    }
}
