using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IBSConnect.AngularApp;
using IBSConnect.Data;
using IBSConnect.ServiceConfiguration;

namespace MapTest;

public static class Startup
{
    public static IServiceProvider GetServices()
    {
        // Get settings from appsettings.json
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("appsettings.json");
        var config = builder.Build();

        var serviceCollection = new ServiceCollection();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole((options) => { })
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information);
        });


        var connectionString = config.GetConnectionString("Property");

        serviceCollection
            .AddLogging(b => b.AddConsole())
            .AddDbContext<IBSConnectDataContext>(options =>
                options
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .EnableDetailedErrors()
                    .UseLoggerFactory(loggerFactory)
            )
            .AddMappers()
            .AddBusiness()
            ;

        return serviceCollection.BuildServiceProvider();
    }
}