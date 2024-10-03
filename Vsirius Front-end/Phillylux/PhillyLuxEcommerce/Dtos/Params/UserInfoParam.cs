namespace PhillyLuxEcommerce.Dtos.Params
{
    public class UserInfoParam
    {
        public string Content { get; set; } = string.Empty;
        public string UserInfo { get; set; } = string.Empty;
        public string CookiesInfo { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        public string Connection { get; set; } = string.Empty;
        public DateTime DateRequest { get; set; }
    }
}
