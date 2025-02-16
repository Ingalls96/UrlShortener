using BlogApp.Data.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<MvcUrlContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("MvcUrlContext")));
}
else
{
    builder.Services.AddDbContext<MvcUrlContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionMvcUrlContext")));
}

//Register ASP.NET Core Identity services
builder.Services.AddIdentity<SiteUser, IdentityRole>(options => {
    options.Password.RequiredLength = 10;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;    
}).AddEntityFrameworkStores<MvcUrlContext>().AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

//register the admin creation
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using(var scope = scopeFactory.CreateScope())
{
    await AuthConfig.ConfigAdmin(scope.ServiceProvider);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
