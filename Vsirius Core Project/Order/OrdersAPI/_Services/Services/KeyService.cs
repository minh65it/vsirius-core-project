using OrdersAPI._Services.Interfaces;

namespace OrdersAPI._Services.Services
{
    public class KeyService : IKeyService
    {
        private readonly Dictionary<string, string> _credentialsDictionary;

        public KeyService(IConfiguration configuration)
        {
            _credentialsDictionary = configuration.GetSection("ApiVsiriusCredentials").Get<Dictionary<string, string>>();
        }

        public bool ValidateCredentials(string clientId, string clientSecret)
        {
            if (_credentialsDictionary["ClientId"] == clientId)
            {
                return _credentialsDictionary["ClientSecret"] == clientSecret;
            }

            return false;
        }
    }
}
