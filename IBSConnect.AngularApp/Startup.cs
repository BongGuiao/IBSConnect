using System;
using IBSConnect.AngularApp.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Targets;
using IBSConnect.Business.Common;
using IBSConnect.Data;
using IBSConnect.ServiceConfiguration;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace IBSConnect.AngularApp;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddDebug()
                //.AddConsole((options) => { })
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information);
        });

        services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

        services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

        // configure DI for application services

        var connectionString = Configuration.GetConnectionString("Database");

        services
            .AddDbContext<IBSConnectDataContext>(options =>
                options
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .EnableDetailedErrors()
                    .UseLoggerFactory(loggerFactory)
            )
            .AddMappers()
            .AddBusiness()
            ;

        services.AddControllers();
        // In production, the Angular files will be served from this directory
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/dist";
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Console.WriteLine(env.ContentRootPath);

        app.UserJsonErrorHandler(env);

        if (env.IsDevelopment())
        {
            //app.UseDeveloperExceptionPage();
        }
        else
        {
            //app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //app.UseHttpsRedirection();
        app.UseStaticFiles();

        if (!env.IsDevelopment())
        {
            app.UseSpaStaticFiles();
        }

        app.UseRouting();

        // custom jwt auth middleware
        app.UseMiddleware<JwtMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            //endpoints.MapControllerRoute(
            //    name: "default",
            //    pattern: "api/{controller}/{action}");
        });

        app.UseSpa(spa =>
        {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            spa.Options.SourcePath = "ClientApp/src";

            if (env.IsDevelopment())
            {
                spa.UseAngularCliServer(npmScript: "start");
            }
        });

        var connectionString = Configuration.GetConnectionString("Database");

        var config = LogManager.Configuration;
        var dbTarget = (DatabaseTarget)config.FindTargetByName("database");
        dbTarget.ConnectionString = connectionString;
        LogManager.ReconfigExistingLoggers();

        var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        logger.Info("Starting IBSConnect...");
    }
}