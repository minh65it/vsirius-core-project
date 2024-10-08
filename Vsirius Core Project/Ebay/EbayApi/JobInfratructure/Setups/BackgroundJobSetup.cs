using EbayApi.JobInfratructure.Dtos;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System;
using Hangfire;

namespace EbayApi.JobInfratructure.Setups
{
    public static class BackgroundJobSetup<T> where T : IJob
    {
        public static async Task Configure()
        {
            var titleJob = typeof(T).Name;

            var configCron = ConfigurationManager.AppSettings[$"Quartz:Cron:{titleJob}"];
            var cron = string.IsNullOrWhiteSpace(configCron) ? "0 30 0/1 * * ?" : configCron;

            JobInfoDto existingDocument = await GetJobInfo(titleJob);

            if (existingDocument == null)
            {
                var newJob = new CreateJobInfo
                {
                    Tittle = titleJob,
                    Description = titleJob,
                    Cron = cron,
                };

                existingDocument = await InsertNewJobAsync(newJob);
            }
            else
            {
                existingDocument.StartDate = DateTime.Now;
                existingDocument.IsRestartDocker = true;

                await UpdateNewJobAsync(existingDocument);
            }

            if (!existingDocument.IsStop)
            {
                RecurringJob.AddOrUpdate<T>(titleJob, job => job.Execute(), existingDocument.Cron);

            }
            else
            {
                RecurringJob.RemoveIfExists(titleJob);
            }
        }

        private static async Task UpdateNewJobAsync(JobInfoDto existingDocument)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = ConfigurationManager.AppSettings["Quartz:API_System"];

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

        private static async Task<JobInfoDto> InsertNewJobAsync(CreateJobInfo newJob)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = ConfigurationManager.AppSettings["Quartz:API_System"];

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

        private static async Task<JobInfoDto> GetJobInfo(string titleJob)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = $"{ConfigurationManager.AppSettings["Quartz:API_System"]}/{titleJob}";
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