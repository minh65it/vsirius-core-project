using IdentityApi.Configurations;
using IdentityApi.Helpers.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VsiriusDataModel.DataContext;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//Add Swagger
builder.Services.AddConfigurationAddSwaggerGen();
//Add IdentityContext
builder.Services.AddDbContext<VsiriusContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Add ConfigurationIdentity
builder.Services.AddConfigurationIdentity(builder.Configuration);

builder.Services.AddCorrelationIdGeneratorService();
builder.Services.AddWriteLog();
//Add RepositoryAccessors and Service
builder.Services.AddDependencyInjectionConfiguration(builder.Configuration);

//Google

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

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Deploy desktop server
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseMvc();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseStaticFiles();
ApplyMigration();
await SeedDataConfig.Run(app);
app.AddCorrelationIdMiddleware();
app.Run();


void ApplyMigration()
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var _db = scope.ServiceProvider.GetRequiredService<VsiriusContext>();

            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }
        }

    }
    catch (Exception ex)
    {

        Console.WriteLine(ex.ToString());
    }
}