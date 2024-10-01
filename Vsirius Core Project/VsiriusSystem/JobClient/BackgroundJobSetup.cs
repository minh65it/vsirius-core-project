using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text;
using Quartz;
using Newtonsoft.Json;

namespace QuartzInfrastructure
{
    public class BackgroundJobSetup<T> : IConfigureOptions<QuartzOptions> where T : IJob
    {
        private readonly IConfiguration _configuration;

        public BackgroundJobSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(QuartzOptions options)
        {
            var titleJob = typeof(T).Name;

            var configCron = _configuration.GetValue<string>($"Quartz:Cron:{titleJob}");
            var cron = string.IsNullOrWhiteSpace(configCron) ? "0 30 0/1 * * ?" : configCron;

            JobInfoDto? existingDocument = GetJobInfo(titleJob).Result;

            if (existingDocument == null)
            {
                var newJob = new CreateJobInfo
                {
                    Tittle = titleJob,
                    Description = titleJob,
                    Cron = cron,
                };

                existingDocument = InsertNewJobAsync(newJob).Result;
            }
            else
            {
                existingDocument.StartDate = DateTime.Now;
                existingDocument.IsRestartDocker = true;

                UpdateNewJobAsync(existingDocument).Wait();
            }

            if (!existingDocument.IsStop)
            {
                var jobKey = JobKey.Create(titleJob);
                options
                .AddJob<T>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithCronSchedule(existingDocument.Cron!)
                        );
            }
        }

        public async Task UpdateNewJobAsync(JobInfoDto existingDocument)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = _configuration["Quartz:API_System"];

                var jsonContent = new StringContent(JsonConvert.SerializeObject(existingDocument), Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync(url, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    ResultDto<JobInfoDto> rs = JsonConvert.DeserializeObject<ResultDto<JobInfoDto>>(content);
                    if (!rs.Success)
                    {
                        throw new Exception("Call API failed");
                    }
                    else
                    {
                        return;
                    }
                }
                throw new Exception("Call API failed");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<JobInfoDto> InsertNewJobAsync(CreateJobInfo? newJob)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = _configuration["Quartz:API_System"];

                var jsonContent = new StringContent(JsonConvert.SerializeObject(newJob), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    ResultDto<JobInfoDto> rs = JsonConvert.DeserializeObject<ResultDto<JobInfoDto>>(content);

                    return rs.Data;
                }
                throw new Exception("Call to API failed");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<JobInfoDto?> GetJobInfo(string titleJob)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = $"{_configuration["Quartz:API_System"]}/{titleJob}";
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var rs = JsonConvert.DeserializeObject<JobInfoDto>(content);
                    return rs;
                }
                throw new Exception("Call API failed");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
