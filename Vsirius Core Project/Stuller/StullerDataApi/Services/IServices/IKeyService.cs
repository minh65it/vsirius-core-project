namespace StullerDataApi.Services.IServices
{
    public interface IKeyService
    {
        bool ValidateCredentials(string clientId, string clientSecret);
    }
}
