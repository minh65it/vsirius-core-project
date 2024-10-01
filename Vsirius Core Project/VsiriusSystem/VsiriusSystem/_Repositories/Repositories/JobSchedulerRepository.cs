using VsiriusSystem._Repositories.Interfaces;
using VsiriusSystem.Data;
using VsiriusSystem.Models;

namespace VsiriusSystem._Repositories.Repositories
{
    public class JobSchedulerRepository : Repository<JobInfo>, IJobSchedulerRepository
    {
        public JobSchedulerRepository(DataContext context) : base(context)
        {
        }
    }
}
