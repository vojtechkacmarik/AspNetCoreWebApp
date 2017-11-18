using System;
using System.Threading;
using AspNetCoreWebApp.Web.Data;
using AspNetCoreWebApp.Web.Models;
using AspNetCoreWebApp.Web.Services;
using AspNetCoreWebApp.Web.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AspNetCoreWebApp.Web
{
    public class Startup
    {
        private const string _secretKey = "MySecret";
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Startup>();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            _logger.LogInformation("Configuration started ...");
            _logger.LogInformation($"UserSecrets: {_secretKey}={Configuration[_secretKey]}");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            ConfigureApplicationLifetime(appLifetime);
            _logger.LogInformation("Configuration completed.");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation($"{services.Count} services are configured.");

            var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTIONSTRING");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString ?? Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            ConfigureApplicationServices(services);

            services.AddOptions();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddMvc();
        }

        private static void ConfigureApplicationServices(IServiceCollection services)
        {
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
        }

        private void ConfigureApplicationLifetime(IApplicationLifetime appLifetime)
        {
            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                appLifetime.StopApplication();
                // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
                eventArgs.Cancel = true;
            };
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted");
            // Perform post-startup activities here
        }

        private void OnStopped()
        {
            _logger.LogInformation("OnStopped");
            // Perform post-stopped activities here
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping");
            // Perform on-stopping activities here
            Log.CloseAndFlush();
            Thread.Sleep(500);
        }
    }
}