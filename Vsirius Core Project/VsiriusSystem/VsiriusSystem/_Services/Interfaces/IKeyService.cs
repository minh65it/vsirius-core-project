namespace VsiriusSystem._Services.Interfaces
{
    public interface IKeyService
    {
        bool ValidateCredentials(string clientId, string clientSecret);
    }
}
