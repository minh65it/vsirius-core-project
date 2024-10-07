using QgoldIntegrationDataApi._Repositories.Interfaces;
using QgoldIntegrationDataApi.Data;
using QgoldIntegrationDataApi.Models;

namespace QgoldIntegrationDataApi._Repositories.Repositories
{
    public class QgoldCsvDataRepository : Repository<QgoldProduct>, IQgoldCsvDataRepository
    {
        public QgoldCsvDataRepository(DBContext context) : base(context)
        {
        }
    }
}
