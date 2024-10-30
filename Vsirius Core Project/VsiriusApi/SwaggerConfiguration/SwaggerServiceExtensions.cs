using Microsoft.OpenApi.Models;

namespace VsiriusApi.SwaggerConfiguration
{
    public static class SwaggerServiceExtensions
    {
        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Identity", new OpenApiInfo { Title = "Identity API", Version = "v1" });
                c.SwaggerDoc("Stuller", new OpenApiInfo { Title = "Stuller API", Version = "v1" });
                c.SwaggerDoc("Qgold", new OpenApiInfo { Title = "Qgold API", Version = "v1" });
                c.SwaggerDoc("SKUMapping", new OpenApiInfo { Title = "SKUMapping API", Version = "v1" });
                c.SwaggerDoc("Order", new OpenApiInfo { Title = "Order API", Version = "v1" });
                c.SwaggerDoc("CSR", new OpenApiInfo { Title = "CSR API", Version = "v1" });
                c.SwaggerDoc("ProductAI", new OpenApiInfo { Title = "ProductAI API", Version = "v1" });
                c.SwaggerDoc("Product", new OpenApiInfo { Title = "Product API", Version = "v1" });
                c.SwaggerDoc("Package", new OpenApiInfo { Title = "Package API", Version = "v1" });
                c.SwaggerDoc("QgoldOrder", new OpenApiInfo { Title = "OrderProcess API", Version = "v1" });
                c.SwaggerDoc("Excluded", new OpenApiInfo { Title = "Excluded API", Version = "v1" });
                c.SwaggerDoc("Pim", new OpenApiInfo { Title = "Pim API", Version = "v1" });
                c.SwaggerDoc("EbayCore", new OpenApiInfo { Title = "EbayCore API", Version = "v1" });
                c.SwaggerDoc("Etsy", new OpenApiInfo { Title = "Etsy API", Version = "v1" });
                c.SwaggerDoc("Walmart", new OpenApiInfo { Title = "Walmart API", Version = "v1" });
                c.SwaggerDoc("Amazon", new OpenApiInfo { Title = "Amazon API", Version = "v1" });
                c.SwaggerDoc("System", new OpenApiInfo { Title = "System API", Version = "v1" });
                c.SwaggerDoc("StullerOrder", new OpenApiInfo { Title = "StullerOrder API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer qazwsx\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    new string[] {}
                }});

                // using System.Reflection for OrderProcess
                //var xmlFilename = "OrderProcessApi.xml";
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    switch (docName)
                    {
                        case "Identity":
                            return apiDesc.GroupName == "Identity";
                        case "Stuller":
                            return apiDesc.GroupName == "Stuller";
                        case "Qgold":
                            return apiDesc.GroupName == "Qgold";
                        case "SKUMapping":
                            return apiDesc.GroupName == "SKUMapping";
                        case "Order":
                            return apiDesc.GroupName == "Order";
                        case "CSR":
                            return apiDesc.GroupName == "CSR";
                        case "ProductAI":
                            return apiDesc.GroupName == "ProductAI";
                        case "Product":
                            return apiDesc.GroupName == "Product";
                        case "Package":
                            return apiDesc.GroupName == "Package";
                        case "QgoldOrder":
                            return apiDesc.GroupName == "QgoldOrder";
                        case "Excluded":
                            return apiDesc.GroupName == "Excluded";
                        case "Pim":
                            return apiDesc.GroupName == "Pim";
                        case "System":
                            return apiDesc.GroupName == "System";
                        case "Etsy":
                            return apiDesc.GroupName == "Etsy";
                        case "EbayCore":
                            return apiDesc.GroupName == "EbayCore";
                        case "Walmart":
                            return apiDesc.GroupName == "Walmart";
                        case "Amazon":
                            return apiDesc.GroupName == "Amazon";
                        case "StullerOrder":
                            return apiDesc.GroupName == "StullerOrder";
                        default:
                            return false;
                    }
                });
            });

        }

        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Identity/swagger.json", "Identity API");
                c.SwaggerEndpoint("/swagger/Stuller/swagger.json", "Stuller API");
                c.SwaggerEndpoint("/swagger/Qgold/swagger.json", "Qgold API");
                c.SwaggerEndpoint("/swagger/SKUMapping/swagger.json", "SKUMapping API");
                c.SwaggerEndpoint("/swagger/ProductAI/swagger.json", "ProductAI API");
                c.SwaggerEndpoint("/swagger/Order/swagger.json", "Order API");
                c.SwaggerEndpoint("/swagger/CSR/swagger.json", "CSR API");
                c.SwaggerEndpoint("/swagger/Product/swagger.json", "Product API");
                c.SwaggerEndpoint("/swagger/Package/swagger.json", "Package API");
                c.SwaggerEndpoint("/swagger/Excluded/swagger.json", "Excluded API");
                c.SwaggerEndpoint("/swagger/Pim/swagger.json", "Pim API");
                c.SwaggerEndpoint("/swagger/EbayCore/swagger.json", "EbayCore API");
                c.SwaggerEndpoint("/swagger/Etsy/swagger.json", "Etsy API");
                c.SwaggerEndpoint("/swagger/Walmart/swagger.json", "Walmart API");
                c.SwaggerEndpoint("/swagger/Amazon/swagger.json", "Amazon API");
                c.SwaggerEndpoint("/swagger/System/swagger.json", "System API");
                c.SwaggerEndpoint("/swagger/StullerOrder/swagger.json", "StullerOrder API");
                c.SwaggerEndpoint("/swagger/QgoldOrder/swagger.json", "QgoldOrder API");
            });
        }
    }
}
