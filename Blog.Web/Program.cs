using System.Configuration;
using Blog.Web.Brokers.Apis;
using Blog.Web.Brokers.DateTimes;
using Blog.Web.Brokers.Loggings;
using Blog.Web.Services.Foundations.Posts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncfusion.Blazor;
using Syncfusion.Licensing;

namespace Blog.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSyncfusionBlazor();
            AddRootDirectory(builder.Services);
            builder.Services.AddLogging();
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IApiBroker, ApiBroker>();
            builder.Services.AddScoped<ILoggingBroker, LoggingBroker>();
            builder.Services.AddScoped<IDateTimeBroker, DateTimeBroker>();
            builder.Services.AddScoped<IPostService, PostService>();

            var app = builder.Build();

            string syncFusionLicenseKey = 
                app.Configuration["Syncfusion:LicenseKey"];

            SyncfusionLicenseProvider.RegisterLicense(syncFusionLicenseKey);


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            app.Run();
        }

        private static void AddRootDirectory(IServiceCollection services)
        {
            services.AddRazorPages(options =>
            {
                options.RootDirectory = "/Views/Pages";
            });
        }
    }
}