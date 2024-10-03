using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors.Infrastructure;
using PhillyLuxEcommerce.Services;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.Configure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
//Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages().AddMvcOptions(options => options.EnableEndpointRouting = false);

builder.Services.AddResponseCaching();

//API Config
builder.Services.AddAPIProduct(builder.Configuration);
builder.Services.AddAPIHome(builder.Configuration);
builder.Services.AddAPISystem(builder.Configuration);
builder.Services.AddAPICart(builder.Configuration);
builder.Services.AddAPINotification(builder.Configuration);
builder.Services.AddAPISearch(builder.Configuration);
builder.Services.AddAPIPayment(builder.Configuration);
builder.Services.AddAPISinglePage(builder.Configuration);
builder.Services.AddAPIUser(builder.Configuration);


//Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//DI
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
builder.Services.AddScoped<ISearchProductService, SearchProductService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<ITopProductService, TopProductService>();

//Login Security
//builder.Services.AddAuthorization();
builder.Configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Routers"))
            .AddJsonFile($"routers.json")
            .Build();
builder.Services.AddHttpClient();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    //.AddCookie(options =>
    //{
    //    //options.LoginPath = "/Home/Login";
    //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // mã hóa cookie
    //    options.Cookie.SameSite = SameSiteMode.None;
    //})
    .AddGoogle(options =>
    {
        //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
        options.SaveTokens = true;
        options.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            context.Response.Redirect(context.RedirectUri + "&prompt=consent");
            return Task.CompletedTask;
        };
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
        facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
        facebookOptions.SaveTokens = true;
        facebookOptions.CallbackPath = "/signin-facebook";
        facebookOptions.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            context.Response.Redirect(context.RedirectUri + "&prompt=consent");
            return Task.CompletedTask;
        };
    })
    ;



//Cookie, Session, Distributed
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.MinimumSameSitePolicy = SameSiteMode.None;
//    options.HttpOnly = HttpOnlyPolicy.Always;
//    options.Secure = CookieSecurePolicy.Always;
//});

var app = builder.Build();
app.UseCors();

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseCookiePolicy();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMvc();

app.UseResponseCaching();

app.MapRazorPages();

app.MapDefaultControllerRoute();

app.UseStatusCodePagesWithReExecute("/Error");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

//test git