using System;

namespace EbayApi.JobInfratructure.Dtos
{
    public class JobInfoDto
    {
        public string Tittle { get; set; }
        public string Description { get; set; }
        public string Cron { get; set; }
        public bool IsRestartDocker { get; set; }
        public bool IsStop { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class CreateJobInfo
    {
        public string Tittle { get; set; }
        public string Description { get; set; }
        public string Cron { get; set; }
    }
}