using Microsoft.EntityFrameworkCore;
using VsiriusDataModel.DataContext;

namespace OrdersAPI.Data
{
    public class DataContext : VsiriusContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
    }
}
