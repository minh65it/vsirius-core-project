using Microsoft.EntityFrameworkCore;
using VsiriusDataModel.DataContext;

namespace CSRApi.Data
{
    public class CSRDbContext : VsiriusDataModel.DataContext.VsiriusContext
    {
        public CSRDbContext(DbContextOptions<VsiriusContext> options) : base(options)
        {
        }
    }
}
