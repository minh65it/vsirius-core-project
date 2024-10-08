namespace EtsyIntegrationDataAPI._Services.Interfaces
{
    public interface ITopOrderService
    {
        Task<List<string>> GetTopListProduct();
    }
}
