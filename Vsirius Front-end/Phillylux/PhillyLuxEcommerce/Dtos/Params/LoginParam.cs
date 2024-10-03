namespace PhillyLuxEcommerce.Dtos.Params
{
    public class LoginParam
    {
        public string Email { get; set; }
        public string Password { get; set; }
        //true: xac thuc, false: chua xac thuc
        public bool IsAuthen { get; set; }
        public string Provider { get; set; }
    }
}
