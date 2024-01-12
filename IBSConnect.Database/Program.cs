using System;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;

namespace IBSConnect.Database;

class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString = config.GetConnectionString("Database");

        EnsureDatabase.For.MySqlDatabase(connectionString);

        var upgrader =
            DeployChanges.To
                .MySqlDatabase(connectionString)
                .WithVariablesDisabled()
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransaction()
                .LogToConsole()
                .ReadyRollLikeJournal("ibs_connect")
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
        return;
    }
}