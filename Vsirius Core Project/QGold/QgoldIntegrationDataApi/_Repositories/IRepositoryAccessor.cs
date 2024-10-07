
using Microsoft.EntityFrameworkCore.Storage;
using QgoldIntegrationDataApi._Repositories.Interfaces;

namespace QgoldIntegrationDataApi._Repositories
{
    public interface IRepositoryAccessor
    {
        public IQgoldCsvDataRepository QgoldCsvDatas { get; }


        Task<bool> SaveChangesAsync();
        public Task<IDbContextTransaction> BeginTransactionAsync();
    }
}