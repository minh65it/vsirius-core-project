using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VsiriusDataModel.DataContext
{
    public class VsiriusContextFactory : IDesignTimeDbContextFactory<VsiriusContext>
    {
        public VsiriusContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<VsiriusContext>();
            var connectionString = "Server=192.168.1.92,1433;Database=VsiriusWeb;User Id=VsiriusServerCompany;Password=vsirius@123; TrustServerCertificate=True;";
            //localhost 
            //var connectionString = "Server=.;Database=VsiriusWeb;User Id=sa;Password=123456; TrustServerCertificate=True;";
            builder.UseSqlServer(connectionString);
            return new VsiriusContext(builder.Options);
        }
    }
}
