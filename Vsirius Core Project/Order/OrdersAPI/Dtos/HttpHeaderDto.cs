namespace OrdersAPI.Dtos
{
    public class HttpHeaderDto
    {
        public string UserAgent { get; set; }
        public DateTime Date { get; set; }
        public string SecChUa { get; set; }
        public string SecChUaMobile { get; set;}
        public string SecChUaPlatform { get; set; }
    }
}
