using VsiriusSystem._Repositories.Interfaces;
using VsiriusSystem.Data;
using VsiriusSystem.Models;

namespace VsiriusSystem._Repositories.Repositories
{
    public class LogRepository : Repository<Log>, ILogRepository
    {
        public LogRepository(DataContext context) : base(context)
        {
        }
    }
}
