namespace QuartzInfrastructure
{
    public class JobInfoDto
    {
        public string Tittle {  get; set; }
        public string? Description {  get; set; }
        public string Cron {  get; set; }
        public bool IsRestartDocker { get; set; }
        public bool IsStop {  get; set; }
        public DateTime StartDate { get; set; }
    }

    public class CreateJobInfo
    {
        public required string Tittle { get; set; }
        public required string Description { get; set; }
        public required string Cron { get; set; }
    }
}
