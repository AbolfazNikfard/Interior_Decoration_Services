using Microsoft.AspNetCore.Identity;
using Interior_Decoration_Services.Data;
using Interior_Decoration_Services.Models;
using Interior_Decoration_Services.PersianTranslationError;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var services = builder.Services;
services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// Retrieve the connection string from an environment variable
string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
services.AddDbContext<ProjectContext>(options =>
{
    //"Data Source=DESKTOP-QDE3PR6;Initial Catalog=Interior_Decoration_DB;Trust Server Certificate=True;Integrated Security=False;User ID=sa;Password=1378529"
    options.UseSqlServer(connectionString);
});


services.AddIdentity<User, IdentityRole>(option =>
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10))
    .AddEntityFrameworkStores<ProjectContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<PersianIdentityErrorDescriber>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAuthorization();
Database_Initializer.Seed(app);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
