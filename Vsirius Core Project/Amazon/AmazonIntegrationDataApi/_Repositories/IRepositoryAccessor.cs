
using AmazonIntegrationDataApi._Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace AmazonIntegrationDataApi._Repositories
{
    public interface IRepositoryAccessor
    {
        public IAmazonJewelryDataFeedItemRepository AmazonJewelryDataFeedItems { get; }


        Task<bool> SaveChangesAsync();
        public Task<IDbContextTransaction> BeginTransactionAsync();

    }
}