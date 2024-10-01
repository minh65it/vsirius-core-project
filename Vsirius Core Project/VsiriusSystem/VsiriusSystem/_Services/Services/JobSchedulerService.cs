using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using System.Text;
using VsiriusSystem._Repositories.Interfaces;
using VsiriusSystem._Services.Interfaces;
using VsiriusSystem.Data;
using VsiriusSystem.Dtos;
using VsiriusSystem.Helpers.Utilities;
using VsiriusSystem.Models;

namespace VsiriusSystem._Services.Services
{
    public class JobSchedulerService : IJobSchedulerService
    {
        private readonly IJobSchedulerRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;

        public JobSchedulerService(
            IJobSchedulerRepository repository,
            IMapper mapper,
            IConfiguration configuration,
            DataContext dataContext)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
            _dataContext = dataContext;
        }

        public async Task<OperationResult> Create(CreateJobInfoDto job)
        {
            var entity = _mapper.Map<JobInfo>(job);
            entity.IsStop = false;
            entity.IsRestartDocker = true;
            entity.StartDate = DateTime.Now;

            _repository.Add(entity);

            var rs = await _repository.SaveChangesAsync();

            if (rs)
            {
                return new OperationResult() { Success = rs, Data = _mapper.Map<JobInfoDto>(entity) };
            }
            else
            {
                return new OperationResult() { Success = rs, Message = "System error please try again" };
            }
        }

        public async Task<OperationResult> Delete(string jobId)
        {
            var entity = await _repository.FindById(jobId);

            _repository.Remove(entity);

            return new OperationResult() { Success = await _repository.SaveChangesAsync() };
        }

        public async Task<PaginationUtility<JobInfoDto>> GetAll(PaginationParam pagination, SearchJobInfoDto search)
        {
            var query = _repository.FindAll();

            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.SearchKey))
                {
                    query = query.Where(x => x.Tittle.Contains(search.SearchKey) || x.Description.Contains(search.SearchKey));
                }
                if (!string.IsNullOrWhiteSpace(search.Cron))
                {
                    query = query.Where(x => x.Cron.Contains(search.Cron));
                }
                if (search.IsStop != null)
                {
                    query = query.Where(x => x.IsStop == search.IsStop);
                }
                if (search.IsRestartDocker != null)
                {
                    query = query.Where(x => x.IsRestartDocker == search.IsRestartDocker);
                }
            }

            var rs = await PaginationUtility<JobInfoDto>.CreateAsync(query.ProjectTo<JobInfoDto>(_mapper.ConfigurationProvider), pagination.PageNumber, pagination.PageSize);

            return rs;
        }

        public async Task<JobInfoDto> GetDetail(string jobId)
        {
            return _mapper.Map<JobInfoDto>(await _repository.FindById(jobId));
        }

        public async Task<OperationResult> Update(JobInfoDto jobInfoDto)
        {
            if (!await _repository.AnyAsync(x => x.Tittle.Equals(jobInfoDto.Tittle)))
            {
                return new OperationResult(false, "Not found job");
            }

            _repository.Update(_mapper.Map<JobInfo>(jobInfoDto));

            try
            {
                await _repository.SaveChangesAsync();
                return new OperationResult(true);
            }
            catch (Exception)
            {
                _dataContext.Database.RollbackTransaction();
                return new OperationResult() { Success = false, Message = "System error please try again" };
            }
        }
        

        public async Task<OperationResult> UpdateJobClient(JobInfoDto jobInfoDto)
        {
            if (!await _repository.AnyAsync(x => x.Tittle.Equals(jobInfoDto.Tittle)))
            {
                return new OperationResult(false, "Not found job");
            }

            _repository.Update(_mapper.Map<JobInfo>(jobInfoDto));

            _dataContext.Database.BeginTransaction();
            try
            {
                await _repository.SaveChangesAsync();
                var checkAPI = await UpdateClient(jobInfoDto);
                if (checkAPI)
                {
                    _dataContext.Database.CommitTransaction();
                    return new OperationResult(true);
                }
                else
                {
                    _dataContext.Database.RollbackTransaction();
                    return new OperationResult(false, "Call api faild");
                };
            }
            catch (Exception)
            {
                _dataContext.Database.RollbackTransaction();
                return new OperationResult() { Success = false, Message = "System error please try again" };
            }
        }
        public async Task<bool> UpdateClient(JobInfoDto jobInfoDto)
        {
            var checkAPI = jobInfoDto.Tittle?.Split("_")[0];
            if (string.IsNullOrEmpty(checkAPI))
            {
                return false;
            }
            string APIUrl = "";
            if (checkAPI.Contains("Amazon"))
            {
                APIUrl = $"{_configuration.GetSection("ApiUpdateJobs:IP").Value}{_configuration.GetSection("ApiUpdateJobs:Amazon").Value}";
            }
            else if (checkAPI.Contains("Ebay"))
            {
                APIUrl = $"{_configuration.GetSection("ApiUpdateJobs:IP").Value}{_configuration.GetSection("ApiUpdateJobs:Ebay").Value}";
            }
            else if (checkAPI.Contains("Esty"))
            {
                APIUrl = $"{_configuration.GetSection("ApiUpdateJobs:IP").Value}{_configuration.GetSection("ApiUpdateJobs:Esty").Value}";
            }
            else if (checkAPI.Contains("PIM"))
            {
                APIUrl = $"{_configuration.GetSection("ApiUpdateJobs:IP").Value}{_configuration.GetSection("ApiUpdateJobs:Pim").Value}";
            }
            else if (checkAPI.Contains("Qgold"))
            {
                APIUrl = $"{_configuration.GetSection("ApiUpdateJobs:IP").Value}{_configuration.GetSection("ApiUpdateJobs:Qgold").Value}";
            }
            else if (checkAPI.Contains("Stuller"))
            {
                APIUrl = $"{_configuration.GetSection("ApiUpdateJobs:IP").Value}{_configuration.GetSection("ApiUpdateJobs:Stuller").Value}";
            }
            else if (checkAPI.Contains("VSiriusWeb"))
            {
                APIUrl = $"{_configuration.GetSection("ApiUpdateJobs:IP").Value}{_configuration.GetSection("ApiUpdateJobs:VsiriusWeb").Value}";
            }
            else if (checkAPI.Contains("Walmart"))
            {
                APIUrl = $"{_configuration.GetSection("ApiUpdateJobs:IP").Value}{_configuration.GetSection("ApiUpdateJobs:Walmart").Value}";
            }
            else
            {
                APIUrl = $"";
            }
            if (APIUrl == "")
                return false;
            //Call API
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                var jsonString = JsonConvert.SerializeObject(jobInfoDto);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(APIUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
