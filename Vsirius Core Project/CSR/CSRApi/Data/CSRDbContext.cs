using Microsoft.EntityFrameworkCore;

namespace CSRApi.Data
{
    public class CSRDbContext : VsiriusDataModel.DataContext.VsiriusContext
    {
        public CSRDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
