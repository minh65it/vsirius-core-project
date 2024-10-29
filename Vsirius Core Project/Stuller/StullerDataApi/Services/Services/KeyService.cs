using StullerDataApi.Services.IServices;

namespace StullerDataApi.Services.Services
{
    public class KeyService : IKeyService
    {
        private readonly Dictionary<string, string> _credentialsDictionary;

        public KeyService(IConfiguration configuration)
        {
            _credentialsDictionary = configuration.GetSection("ApiVsiriusCredentialsStuller").Get<Dictionary<string, string>>();
        }

        public bool ValidateCredentials(string clientId, string clientSecret)
        {
            if (_credentialsDictionary.TryGetValue(clientId, out var expectedSecret))
            {
                return clientSecret == expectedSecret;
            }

            return false;
        }
    }
}
