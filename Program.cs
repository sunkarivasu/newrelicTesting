using newRelicTestingApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.Extensions.Logging.ApplicationInsights;


var builder = WebApplication.CreateBuilder(args);

// Configure Logging 
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    //logging.AddDebug();
});

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.Configure<AzureFileLoggerOptions>(options => {
//    options.FileName = "logs-";
//    options.FileSizeLimit = 50 * 1024;
//    options.RetainedFileCountLimit = 5;
//});

//Configure logging to Azure filesystem
builder.Logging.AddAzureWebAppDiagnostics();

var config = builder.Configuration;

// connecting to DB
//builder.Services.AddDbContext<testingContext>(opt => opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
