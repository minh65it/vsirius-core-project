using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text;
using Quartz;
using Newtonsoft.Json;
using System.Data.SqlClient;

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
                /*var httpClient = new HttpClient();
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
                throw new Exception("Call API failed");*/
                var connectionString = _configuration.GetConnectionString("SystemConnection");
                var rs = new JobInfoDto();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = _configuration.GetSection("Connectionstrings:QueryJob_Update").Value!.ToString();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Tittle", existingDocument.Tittle);
                        command.Parameters.AddWithValue("@Description", existingDocument.Description);
                        command.Parameters.AddWithValue("@Cron", existingDocument.Cron);
                        command.Parameters.AddWithValue("@IsRestartDocker", existingDocument.IsRestartDocker);
                        command.Parameters.AddWithValue("@IsStop", existingDocument.IsStop);
                        command.Parameters.AddWithValue("@StartDate", existingDocument.StartDate);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            connection.Close();
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<JobInfoDto> InsertNewJobAsync(CreateJobInfo? newJob)
        {
            /*try
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
            }*/
            var connectionString = _configuration.GetConnectionString("SystemConnection");
            var rs = new JobInfoDto();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = _configuration.GetSection("Connectionstrings:QueryJob_Insert").Value!.ToString();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Tittle", newJob.Tittle);
                    command.Parameters.AddWithValue("@Description", newJob.Description);
                    command.Parameters.AddWithValue("@Cron", newJob.Cron);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        connection.Close();
                        return rs;
                    }
                }
            }
            return rs;
        }

        public async Task<JobInfoDto?> GetJobInfo(string titleJob)
        {
            try
            {
                /*var httpClient = new HttpClient();
                var url = $"{_configuration["Quartz:API_System"]}/{titleJob}";
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var rs = JsonConvert.DeserializeObject<JobInfoDto>(content);
                    return rs;
                }
                throw new Exception("Call API failed");*/
                var connectionString = _configuration.GetConnectionString("SystemConnection");
                var rs = new JobInfoDto();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = _configuration.GetSection("Connectionstrings:QueryJob_Select").Value!.ToString() + "'" + titleJob + "'" ;
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                rs.Tittle = reader["Tittle"].ToString() ?? string.Empty;
                                rs.Description = reader["Description"].ToString() ?? string.Empty;
                                rs.Cron = reader["Cron"].ToString() ?? string.Empty;
                                rs.IsRestartDocker = reader["IsRestartDocker"].ToString()!.Equals("1");
                                rs.IsStop = reader["IsStop"].ToString()!.Equals("1");
                                rs.StartDate = DateTime.Parse(reader["StartDate"].ToString()!);
                            }
                            connection.Close();
                        }
                    }
                }
                return rs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }
}
