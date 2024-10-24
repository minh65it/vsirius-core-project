using Microsoft.EntityFrameworkCore;
using VsiriusDataModel.DataContext;

namespace CSRApi.Data
{
    public class CSRContext : VsiriusContext
    {
        public CSRContext(DbContextOptions<VsiriusContext> options) : base(options)
        {
        }
    }
}
