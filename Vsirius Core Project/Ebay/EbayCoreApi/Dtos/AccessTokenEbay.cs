namespace EbayCoreApi.Dtos
{
    public class AccessTokenEbay
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }
    public class AccessTimeout
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public DateTime created_at { get; set; }
    }
}