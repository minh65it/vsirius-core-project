namespace VsiriusSystem.Helpers.Params
{
    public class LogParams
    {
        public string? ProjectName { get; set; }
        public string? URL { get; set;}
        public LogLevel? Level { get; set;}
        public string? CorrelationId { get; set;}
        /// <summary>
        /// MM/dd/YY
        /// </summary>
        public string? TimeStampFrom { get; set; }
        /// <summary>
        /// MM/dd/YY
        /// </summary>
        public string? TimeStampTo { get; set; }
    }
}
