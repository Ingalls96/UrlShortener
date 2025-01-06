using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;

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

//Seed Default User


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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
