namespace VsiriusSystem.Models
{
    public class Log
    {
        public Guid LogId { get; set; }
        public string? Message { get; set; }
        public string? Level { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string? Exception { get; set; }
        public string? URL { get; set; }
        public string? ProjectName { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
