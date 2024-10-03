using IdentityApi.Data;
using IdentityApi.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IdentityApi.Configurations
{
    public static class ConfigurationIdentity
    {
        public static void AddConfigurationIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException();
            // add Identity
            services.AddIdentity<VsiriusUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = false;
                opt.Password.RequireDigit = false;
                opt.Tokens.EmailConfirmationTokenProvider = "emailconfirmationByVsirius";
            })
             .AddEntityFrameworkStores<IdentityContext>()
             .AddDefaultTokenProviders()
             .AddTokenProvider<EmailConfirmationTokenProvider<VsiriusUser>>("emailconfirmationByVsirius")
             .AddTokenProvider<PhoneNumberTokenProvider<VsiriusUser>>("tokePhonebyVsirius");

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(1));
            services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromDays(1));

            // add authentication
            services.AddAuthentication(x =>
            {
                //xác nhận danh tính với JwtBearerDefaults.AuthenticationScheme
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
                options.AddPolicy("AdminOrManager", policy => policy.RequireRole("Admin", "Manager"));
                options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));

            });
            services.Configure<PhoneNumberTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(10);
                options.TokenLength = 6;
                options.MessageFormat = "Your authentication code is {0}.";
                options.Name = "MyPhoneProvider";
            });

        }
    }
}
