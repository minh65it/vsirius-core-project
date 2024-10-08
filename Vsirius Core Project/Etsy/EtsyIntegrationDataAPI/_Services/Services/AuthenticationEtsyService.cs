using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Dtos.AuthenticationEtsyDto;
using EtsyIntegrationDataAPI.Models;
using EtsyIntegrationDataAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace EtsyIntegrationDataAPI._Services.Services
{
    public class AuthenticationEtsyService : IAuthenticationEtsyService
    {
        private readonly IConfiguration _config;
        private readonly IAuthenEtsyRepository _authenEtsyRepository;
        private readonly bool _isProduction = false;

        public AuthenticationEtsyService(IConfiguration config, IAuthenEtsyRepository authenEtsyRepository)
        {
            _config = config;
            _isProduction = _config.GetSection("Etsy:Environment").Value == "Production" ? true : false;
            _authenEtsyRepository = authenEtsyRepository;
        }

        public async Task<OperationResult> GrantAccess(string code, string state)
        {
            var dataAuthen = await _authenEtsyRepository.FindAll(x => x.CreatedDate >= DateTime.Now.AddHours(-1)).FirstOrDefaultAsync();
            if (dataAuthen is null)
            {
                AuthenEtsy authen = new AuthenEtsy();
                authen.CodeChallenge = null;
                authen.CodeVerifier = null;
                authen.State = state;
                authen.Scopes = null;
                authen.Code = code;
                authen.CreatedDate = DateTime.Now;
                _authenEtsyRepository.Add(authen);
                await _authenEtsyRepository.SaveChangesAsync();
                return new OperationResult() { Success = true };
            }
            dataAuthen.Code = code;
            dataAuthen.State = state;
            _authenEtsyRepository.Update(dataAuthen);
            await _authenEtsyRepository.SaveChangesAsync();
            return new OperationResult() { Success = true };
        }

        public async Task<OAuthTokenDto> RefreshToken(string? refreshToken)
        {
            var authen = await _authenEtsyRepository.FindAll(x => x.RefreshToken != null).OrderByDescending(x => x.UpdatedDate).FirstOrDefaultAsync();
            if (authen is null)
            {
                var tokenAccessData = await RequestAnAccessToken(null, null);
                var tokenAccess = (OAuthTokenDto)tokenAccessData.Data;
                return tokenAccess;
            }
            else if (authen.UpdatedDate >= DateTime.Now.AddHours(-1))
            {
                return new OAuthTokenDto()
                {
                    Access_token = authen.AccessToken ?? "",
                    Refresh_token = authen.RefreshToken ?? "",
                };
            }
            else
            {
                var tokenData = await RequestingARefreshToken(authen.RefreshToken!);
                var token = (OAuthTokenDto)tokenData.Data;
                if (token != null)
                {
                    return token;
                }
                else
                {
                    var tokenAccessData = await RequestAnAccessToken(null, null);
                    var tokenAccess = (OAuthTokenDto)tokenAccessData.Data;
                    return tokenAccess;
                }
            }
        }

        public async Task<OperationResult> RequestAnAccessToken(string? code, string? code_verifier)
        {
            //Get database
            var dataAuthen = await _authenEtsyRepository.FirstOrDefaultAsync(x => x.CreatedDate >= DateTime.Now.AddHours(-1));
            if (dataAuthen is null)
                return new OperationResult(success: false, "Not record AuthenEtsy in database");
            //Call API
            using HttpClient client = new();

            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetSection("Etsy:RequestToken").Value!);
            var parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", _config.GetSection("Etsy:KeysString").Value!),
                    new KeyValuePair<string, string>("redirect_uri", _config.GetSection("Etsy:RedirectUri").Value!),
                    new KeyValuePair<string, string>("code", code ?? dataAuthen.Code!),
                    new KeyValuePair<string, string>("code_verifier", code_verifier ?? dataAuthen.CodeVerifier!)

                };
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            request.Content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<OAuthTokenDto>(responseBody);

                if (data != null)
                {
                    dataAuthen.RefreshToken = data.Refresh_token;
                    dataAuthen.AccessToken = data.Access_token;
                    dataAuthen.UpdatedDate = DateTime.Now;
                    _authenEtsyRepository.Update(dataAuthen);
                    await _authenEtsyRepository.SaveChangesAsync();
                }

                return new OperationResult(success: true, "Success Request An Access Token", data: data);
            }
            return new OperationResult(success: false, "Request An Access Token Error");
        }

        public async Task<OperationResult> RequestAnAuthorizationCode(List<string> scope)
        {
            if (!scope.Any())
            {
                return new OperationResult(success: true, message: "Scope is null");
            }
            ConnectEtsyDto connectEtsyDto = new ConnectEtsyDto();
            connectEtsyDto.Client_id = _config.GetSection("Etsy:KeysString").Value!;
            connectEtsyDto.Redirect_uri = _config.GetSection("Etsy:RedirectUri").Value!;
            connectEtsyDto.Scope = string.Join("%20", scope);
            //Gen State
            Random random = new Random();
            connectEtsyDto.State = "superstate";
            //Gen CodeVerifier
            string codeVerifier = Pkce.GenCodeVerifier();
            connectEtsyDto.Code_challenge = Pkce.GenerateCodeChallenge(codeVerifier);

            //Call API
            using HttpClient client = new();
            var url = _config.GetSection("Etsy:OAuthConnect").Value!;
            var parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("response_type", connectEtsyDto.Response_type),
                    new KeyValuePair<string, string>("client_id", connectEtsyDto.Client_id),
                    new KeyValuePair<string, string>("redirect_uri", connectEtsyDto.Redirect_uri),
                    new KeyValuePair<string, string>("scope", connectEtsyDto.Scope),
                    new KeyValuePair<string, string>("state", connectEtsyDto.State),
                    new KeyValuePair<string, string>("code_challenge", connectEtsyDto.Code_challenge),
                    new KeyValuePair<string, string>("code_challenge_method", connectEtsyDto.Code_challenge_method),

                };

            var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
            url = $"{url}?{queryString}";
            HttpResponseMessage response = await client.GetAsync(_config.GetSection("Etsy:OAuthConnect").Value!);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                AuthenEtsy authen = new AuthenEtsy();
                authen.CodeChallenge = connectEtsyDto.Code_challenge;
                authen.CodeVerifier = codeVerifier;
                authen.State = connectEtsyDto.State;
                authen.Scopes = connectEtsyDto.Scope;
                authen.CreatedDate = DateTime.Now;
                _authenEtsyRepository.Add(authen);
                await _authenEtsyRepository.SaveChangesAsync();
                return new OperationResult(success: true, url, data: new { codeVerifier = codeVerifier, state = connectEtsyDto.State });
            }
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                AuthenEtsy authen = new AuthenEtsy();
                authen.CodeChallenge = connectEtsyDto.Code_challenge;
                authen.CodeVerifier = codeVerifier;
                authen.State = connectEtsyDto.State;
                authen.Scopes = connectEtsyDto.Scope;
                authen.CreatedDate = DateTime.Now;
                _authenEtsyRepository.Add(authen);
                await _authenEtsyRepository.SaveChangesAsync();
                return new OperationResult(success: true, url, data: new { codeVerifier = codeVerifier, state = connectEtsyDto.State });
            }
        }

        public async Task<OperationResult> RequestingARefreshToken(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return new OperationResult(success: false, "Refresh Token is null");
            //Call API
            using HttpClient client = new();

            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetSection("Etsy:RequestToken").Value!);
            var parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("client_id", _config.GetSection("Etsy:KeysString").Value!),
                    new KeyValuePair<string, string>("refresh_token", refreshToken)

                };
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            request.Content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<OAuthTokenDto>(responseBody);
                if (data != null)
                {
                    var authen = await _authenEtsyRepository.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
                    if (authen != null)
                    {
                        authen.RefreshToken = data.Refresh_token;
                        authen.AccessToken = data.Access_token;
                        authen.OldRefreshToken = refreshToken;
                        authen.UpdatedDate = DateTime.Now;
                        _authenEtsyRepository.Update(authen);
                    }
                    else
                    {
                        _authenEtsyRepository.Add(new AuthenEtsy()
                        {
                            RefreshToken = data.Refresh_token,
                            AccessToken = data.Access_token,
                            OldRefreshToken = refreshToken,
                            CreatedDate = DateTime.Now
                        });
                    }
                    await _authenEtsyRepository.SaveChangesAsync();
                }
                return new OperationResult(success: true, "Success Requesting A Refresh Token", data: data);
            }
            return new OperationResult(success: false, "Requesting A Refresh Token Error");
            throw new NotImplementedException();
        }
    }
}
