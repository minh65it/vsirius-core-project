using Microsoft.OpenApi.Models;
using System.Reflection;

namespace EbayCoreApi.Common.Configurations
{
    public static class SwaggerGenConfig
    {
        public static void AddConfigurationAddSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Ebay API",
                Description = "",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme." +
                "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
                "\r\n\r\n Example: \"Bearer qazwsx\"",
            });
            c.AddSecurityRequirement(
            new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
            });
            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
             });
        }
    }
}
