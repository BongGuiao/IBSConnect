using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IBSConnect.Business.Common;
using IBSConnect.Business.Models;
using IBSConnect.Data;
using IBSConnect.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace IBSConnect.Business;

public class MetaDataBL : IMetaDataBL
{
    private readonly IBSConnectDataContext _dataContext;
    private readonly IMapper _mapper;

    private readonly AppSettings _appSettings;

    public MetaDataBL(IBSConnectDataContext dataContext, IOptions<AppSettings> appSettings, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _appSettings = appSettings.Value;
    }

    public async Task<int> AddApplicationAsync(string name)
    {
        var entity = _dataContext.Applications.Add(new Application() { Name = name });

        await _dataContext.SaveChangesAsync();

        return entity.Entity.Id;
    }

    public async Task UpdateApplicationAsync(int id, string name)
    {
        var app = await _dataContext.Applications.FindAsync(id);

        app.Name = name;

        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteApplicationAsync(int id)
    {
        var app = await _dataContext.Applications.FindAsync(id);

        _dataContext.Remove(app);

        await TrySaveChangesAsync("Application");
    }

    public async Task<IEnumerable<ItemViewModel>> GetApplicationsAsync()
    {
        return await _dataContext.Applications.Select(a => new ItemViewModel()
            {
                Id = a.Id,
                Name = a.Name
            })
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<int> AddUnitAreaAsync(string name)
    {
        var entity = _dataContext.UnitAreas.Add(new UnitArea() { Name = name });

        await _dataContext.SaveChangesAsync();

        return entity.Entity.Id;
    }

    public async Task UpdateUnitAreaAsync(int id, string name)
    {
        var app = await _dataContext.UnitAreas.FindAsync(id);

        app.Name = name;

        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteUnitAreaAsync(int id)
    {
        var app = await _dataContext.UnitAreas.FindAsync(id);

        _dataContext.Remove(app);

        await TrySaveChangesAsync("UnitArea");
    }

    public async Task<IEnumerable<ItemViewModel>> GetUnitAreasAsync()
    {
        return await _dataContext.UnitAreas.Select(a => new ItemViewModel()
            {
                Id = a.Id,
                Name = a.Name
            })
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<int> AddCategoryAsync(string name, bool isFreeTier)
    {
        var entity = _dataContext.Categories.Add(new Category() { Name = name, IsFreeTier = isFreeTier });

        await _dataContext.SaveChangesAsync();

        return entity.Entity.Id;
    }

    public async Task UpdateCategoryAsync(int id, string name, bool isFreeTier)
    {
        var app = await _dataContext.Categories.FindAsync(id);

        app.Name = name;
        app.IsFreeTier = isFreeTier;

        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var app = await _dataContext.Categories.FindAsync(id);

        _dataContext.Remove(app);

        await TrySaveChangesAsync("Category");
    }

    public async Task<IEnumerable<CategoryItemViewModel>> GetCategoriesAsync()
    {
        return await _dataContext.Categories.Select(a => new CategoryItemViewModel()
        {
            Id = a.Id,
            Name = a.Name,
            IsFreeTier = a.IsFreeTier
        })
            .OrderBy(a => a.Name)
            .ToListAsync();
    }


    public async Task<int> AddYearAsync(string name)
    {
        var entity = _dataContext.Years.Add(new Year() { Name = name });

        await _dataContext.SaveChangesAsync();

        return entity.Entity.Id;
    }

    public async Task UpdateYearAsync(int id, string name)
    {
        var app = await _dataContext.Years.FindAsync(id);

        app.Name = name;

        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteYearAsync(int id)
    {
        var app = await _dataContext.Years.FindAsync(id);

        _dataContext.Remove(app);

        await TrySaveChangesAsync("Year");
    }

    public async Task<IEnumerable<ItemViewModel>> GetYearsAsync()
    {
        return await _dataContext.Years.Select(a => new ItemViewModel()
        {
            Id = a.Id,
            Name = a.Name
        }).OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<int> AddCourseAsync(string name)
    {
        var entity = _dataContext.Courses.Add(new Course() { Name = name });

        await _dataContext.SaveChangesAsync();

        return entity.Entity.Id;
    }

    public async Task UpdateCourseAsync(int id, string name)
    {
        var app = await _dataContext.Courses.FindAsync(id);

        app.Name = name;

        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteCourseAsync(int id)
    {
        var app = await _dataContext.Courses.FindAsync(id);

        _dataContext.Remove(app);

        await TrySaveChangesAsync("Course");
    }

    public async Task<IEnumerable<ItemViewModel>> GetCoursesAsync()
    {
        return await _dataContext.Courses.Select(a => new ItemViewModel()
        {
            Id = a.Id,
            Name = a.Name
        }).OrderBy(a => a.Name)
           .ToListAsync();
    }

    public async Task<int> AddSectionAsync(string name)
    {
        var entity = _dataContext.Sections.Add(new Section() { Name = name });

        await _dataContext.SaveChangesAsync();

        return entity.Entity.Id;
    }

    public async Task UpdateSectionAsync(int id, string name)
    {
        var app = await _dataContext.Sections.FindAsync(id);

        app.Name = name;

        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteSectionAsync(int id)
    {
        var app = await _dataContext.Sections.FindAsync(id);

        _dataContext.Remove(app);

        await _dataContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ItemViewModel>> GetSectionsAsync()
    {
        return await _dataContext.Sections.Select(a => new ItemViewModel()
        {
            Id = a.Id,
            Name = a.Name
        }).ToListAsync();
    }


    public async Task<int> AddCollegeAsync(string name)
    {
        var entity = _dataContext.Colleges.Add(new College() { Name = name });

        await _dataContext.SaveChangesAsync();

        return entity.Entity.Id;
    }

    public async Task UpdateCollegeAsync(int id, string name)
    {
        var app = await _dataContext.Colleges.FindAsync(id);

        app.Name = name;

        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteCollegeAsync(int id)
    {
        var app = await _dataContext.Colleges.FindAsync(id);

        _dataContext.Remove(app);

        await TrySaveChangesAsync("College");
    }

    public async Task<IEnumerable<ItemViewModel>> GetCollegesAsync()
    {
        return await _dataContext.Colleges.Select(a => new ItemViewModel()
        {
            Id = a.Id,
            Name = a.Name
        }).OrderBy(a => a.Name)
            .ToListAsync();
    }

    private async Task TrySaveChangesAsync(string name)
    {
        try
        {
            await _dataContext.SaveChangesAsync();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException e) when (e.InnerException is MySqlException mySqlException)
        {
            if (mySqlException.ErrorCode == MySqlErrorCode.RowIsReferenced)
            {
                throw new IBSConnectException($"{name} is in use");
            }
        }
    }
}