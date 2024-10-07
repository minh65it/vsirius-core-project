
using Microsoft.EntityFrameworkCore.Storage;
using QgoldIntegrationDataApi._Repositories.Interfaces;
using QgoldIntegrationDataApi._Repositories.Repositories;
using QgoldIntegrationDataApi.Data;

namespace QgoldIntegrationDataApi._Repositories
{
    public class RepositoryAccessor : IRepositoryAccessor
    {
        private DBContext _context;


        public RepositoryAccessor(DBContext context)
        {
            _context = context;

            QgoldCsvDatas = new QgoldCsvDataRepository(_context);
        }
        public IQgoldCsvDataRepository QgoldCsvDatas { get; set; }

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