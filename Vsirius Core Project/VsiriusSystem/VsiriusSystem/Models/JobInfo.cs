using System.ComponentModel.DataAnnotations;

namespace VsiriusSystem.Models
{
    public class JobInfo
    {
        [Key]
        public required string Tittle { get; set; }
        public string? Description { get; set; }
        public required string Cron { get; set; }
        public bool IsRestartDocker { get; set; }
        public bool IsStop { get; set; }
        public DateTime StartDate { get; set; }
    }
}
