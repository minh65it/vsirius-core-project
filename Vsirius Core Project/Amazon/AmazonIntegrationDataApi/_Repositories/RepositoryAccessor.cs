
using Microsoft.EntityFrameworkCore.Storage;
using AmazonIntegrationDataApi._Repositories;
using AmazonIntegrationDataApi.Data;
using AmazonIntegrationDataApi._Repositories.Interfaces;
using AmazonIntegrationDataApi._Repositories.Repositories;

namespace DBDataContext._Repositories
{
    public class RepositoryAccessor : IRepositoryAccessor
    {
        private DBContext _context;


        public RepositoryAccessor(DBContext context)
        {
            _context = context;

            AmazonJewelryDataFeedItems = new AmazonJewelryDataFeedItemRepository(_context);
        }
        public IAmazonJewelryDataFeedItemRepository AmazonJewelryDataFeedItems { get; private set; }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

    }
}