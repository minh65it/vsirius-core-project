using EtsyIntegrationDataAPI.Dtos.AuthenticationEtsyDto;
using EtsyIntegrationDataAPI.Utilities;

namespace EtsyIntegrationDataAPI._Services.Interfaces
{
    public interface IAuthenticationEtsyService
    {
        public Task<OperationResult> RequestAnAuthorizationCode(List<string> scope);
        public Task<OperationResult> GrantAccess(string code, string state);
        public Task<OperationResult> RequestAnAccessToken(string? code, string? code_verifier);
        public Task<OperationResult> RequestingARefreshToken(string refreshToken);
        public Task<OAuthTokenDto> RefreshToken(string? refreshToken);
    }
}
