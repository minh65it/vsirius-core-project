using IdentityModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EtsyIntegrationDataAPI.Utilities
{
    public static class Pkce
    {

        public static string GenCodeVerifier()
        {
            var codeVerifier = CryptoRandom.CreateUniqueId(96); // A PKCE works by using a code verifier , which must be a high-entropy random string consisting of between 43 and 128 characters from the range [A-Za-z0-9._~-] (i.e. unreserved URI characters).
            return codeVerifier;
        }
        public static string GenerateCodeChallenge(string codeVerifier)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            var b64Hash = Convert.ToBase64String(hash);
            var code = Regex.Replace(b64Hash, "\\+", "-");
            code = Regex.Replace(code, "\\/", "_");
            code = Regex.Replace(code, "=+$", "");
            return code;
        }
    }
}
