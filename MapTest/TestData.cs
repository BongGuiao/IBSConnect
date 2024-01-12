using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using IBSConnect.Business;
using IBSConnect.Business.Models;
using RandomNameGeneratorLibrary;

namespace MapTest;

public class TestData
{
    static Random r = new Random();
    private readonly IServiceProvider _services;
        
    public TestData(IServiceProvider services)
    {
        _services = services;

        var personGenerator = new PersonNameGenerator();
    }

    public async Task CreateTestData(Func<TestData, Task> executeFuncs)
    {
        Console.WriteLine("Creating Test Data...");
        await executeFuncs(this);
    }

    public async Task<int> GetUser(string username)
    {
        var userBl = _services.GetService<IUserBL>();

        var user = await userBl.GetByUserNameAsync(username);

        return user.Id;
    }

    public async Task<int> CreatUser(string username, string password, string firstname, string lastname, string middlename, string role)
    {
        var userBl = _services.GetService<IUserBL>();

        var id = await userBl.CreateAsync(new UserViewModel()
        {
            Username = username,
            Password = password,
            FirstName = firstname,
            LastName = lastname,
            MiddleName = middlename,
            Role = role

        });

        return id;
    }

}