using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using IBSConnect.Business;
using IBSConnect.Business.Models;

namespace MapTest;

class Program
{
    static IServiceProvider services;

    static async Task Main(string[] args)
    {
        services = Startup.GetServices();

        //await BuildTestData();

        Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("password123"));

        //var member = services.GetService<IMemberBL>();
        //var session = services.GetService<IMemberSessionBL>();
        //var mem = await member.GetByUserNameAsync("user0001");
        //await session.EndSessionAsync(mem.Id);

        //var member = services.GetService<IMemberBL>();
        //var session = services.GetService<IMemberSessionBL>();
        //var meta = services.GetService<IMetaDataBL>();

        //await meta.AddApplicationAsync("Word");
        //await meta.AddApplicationAsync("Excel");
        //await meta.AddApplicationAsync("PowerPoint");

        //await meta.AddCategoryAsync("Student");
        //await meta.AddYearAsync("1");
        //await meta.AddCourseAsync("ECE");
        //await meta.AddSectionAsync("A");
        //await meta.AddCollegeAsync("ENG");


        //await member.CreateAsync(new CreateMemberRequest()
        //{
        //    FirstName = "A",
        //    MiddleName = "B",
        //    LastName = "C",
        //    Password = "P",
        //    Username = "user0001",
        //    CategoryId = 1,
        //    CollegeId = 1,
        //    CourseId = 1,
        //    SectionId = 1,
        //    YearId = 1
        //});

        //var apps = await meta.GetApplicationsAsync();
        //var mem = await member.GetByUserNameAsync("user0001");

        //await member.CreditMinutesAsync(mem.Id, 20);

        //await session.StartSessionAsync(mem.Id, apps.Select(a => a.Id));

        //Console.WriteLine("Press any key to load test data");
        //Console.ReadLine();


        Console.ReadLine();
    }

    static async Task BuildTestData()
    {
        try
        {
            var testData = new TestData(services);

            await testData.CreateTestData(async t =>
            {
                await t.CreatUser("admin", "password", "Admin", "Strator", "Ni", "Administrator");
                await t.CreatUser("mcango", "password", "Marion", "Cango", "A", "Administrator");
                await t.CreatUser("dsantos", "password", "David Khristepher", "Santos", "Ramos", "Administrator");

                //var userId = await t.GetUser("admin");
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed!");
            Console.ResetColor();
            throw;
        }
    }

}