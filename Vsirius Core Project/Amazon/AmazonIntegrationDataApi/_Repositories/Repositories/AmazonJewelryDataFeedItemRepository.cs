using AmazonIntegrationDataApi._Repositories.Interfaces;
using AmazonIntegrationDataApi.Data;
using AmazonIntegrationDataApi.Models;

namespace AmazonIntegrationDataApi._Repositories.Repositories
{
    public class AmazonJewelryDataFeedItemRepository : Repository<AmazonJewelryDataFeedItemV3>, IAmazonJewelryDataFeedItemRepository
    {
        public AmazonJewelryDataFeedItemRepository(DBContext context) : base(context)
        {
        }
    }
}
