using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Add configuration for Google OAuth client
builder.Configuration.AddJsonFile("appsettings.json");
var configuration = builder.Configuration.GetSection("Authentication:Google");

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
    // Retrieve ClientId and ClientSecret from configuration
    googleOptions.ClientId = configuration["ClientId"];
    googleOptions.ClientSecret = configuration["ClientSecret"];
});

// Add services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Add routing middleware
app.UseRouting();

// Add authentication middleware
app.UseAuthentication();

// Add authorization middleware
app.UseAuthorization();

// Configure endpoints
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
