
namespace IdentityApi.Configurations
{
    public class PhoneNumberTokenProviderOptions
    {
        public TimeSpan TokenLifespan { get; set; } = TimeSpan.FromMinutes(10);
        public int TokenLength { get; set; } = 6;
        public string MessageFormat { get; set; } = "Your authentication code is {0}.";
        public string Name { get; set; } = "MyPhoneProvider";
    }
}
