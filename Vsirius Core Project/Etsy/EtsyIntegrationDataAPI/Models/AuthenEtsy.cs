using System.ComponentModel.DataAnnotations;

namespace EtsyIntegrationDataAPI.Models
{
    public class AuthenEtsy
    {
        [Key]
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? CodeChallenge { get; set; }
        public string? CodeVerifier { get; set; }
        public string? State { get; set; }
        public string? Scopes { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? OldAccessToken { get; set; }
        public string? OldRefreshToken { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
