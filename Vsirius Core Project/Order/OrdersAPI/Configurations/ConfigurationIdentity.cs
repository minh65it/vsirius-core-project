using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OrdersAPI.Configurations
{
    public static class ConfigurationIdentity
    {
        public static void AddConfigurationIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException();

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
        }
    }
}
