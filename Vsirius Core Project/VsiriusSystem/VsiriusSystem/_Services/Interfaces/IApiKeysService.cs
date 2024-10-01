namespace VsiriusSystem._Services.Interfaces
{
    public interface IApiKeysService
    {
        bool ValidateCredentials(string clientId, string clientSecret);
        bool ValidateCredentialsBackOffice(string clientSecret);
    }
}
