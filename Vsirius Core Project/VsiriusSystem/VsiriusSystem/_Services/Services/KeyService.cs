using VsiriusSystem._Services.Interfaces;

namespace VsiriusSystem._Services.Services
{
    public class KeyService : IKeyService
    {
        private readonly Dictionary<string, string> _credentialsDictionary;

        public KeyService(IConfiguration configuration)
        {
            _credentialsDictionary = configuration.GetSection("CredentialsAPI").Get<Dictionary<string, string>>()!;
        }

        public bool ValidateCredentials(string? clientId, string? clientSecret)
        {
            try
            {
                if (_credentialsDictionary.TryGetValue(clientId, out var expectedSecret))
                {
                    return clientSecret == expectedSecret;
                }         
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}
