using IdentityApi.Configurations;
using IdentityApi.Data;
using IdentityApi.Helpers.Utilities;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//Add Swagger
builder.Services.AddConfigurationAddSwaggerGen();
//Add IdentityContext
builder.Services.AddDbContext<IdentityContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Add ConfigurationIdentity
builder.Services.AddConfigurationIdentity(builder.Configuration);

//Add RepositoryAccessors and Service
builder.Services.AddDependencyInjectionConfiguration(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddRouting();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

//Add Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

SeedDataConfig.Run(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Deploy desktop server
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();
app.UseRouting();
app.UseMvc();
app.UseMiddleware<ExceptionHandlingMiddleware>();
//app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseStaticFiles();
app.Run();