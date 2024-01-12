using System.Collections.Generic;
using System.Threading.Tasks;
using IBSConnect.Business;
using IBSConnect.Business.Common;
using IBSConnect.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace IBSConnect.AngularApp.Controllers;

public class Item
{
    public string Name { get; set; }
}

public class CategoryItem
{
    public string Name { get; set; }
    public bool IsFreeTier { get; set; }
}


[ApiController]
[Route("api/[controller]")]
public class MetaDataController : AuthorizedUserControllerBase
{
    private IMetaDataBL _metaDataBl;

    public MetaDataController(IMetaDataBL metaDataBl)
    {
        _metaDataBl = metaDataBl;
    }

    [Authorize(Roles.Administrator)]
    #region Applications
    [HttpPost("applications")]
    public async Task<int> AddApplicationAsync([FromBody] Item item)
    {
        return await _metaDataBl.AddApplicationAsync(item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("applications/{id}")]
    public async Task UpdateApplicationAsync(int id, [FromBody] Item item)
    {
        await _metaDataBl.UpdateApplicationAsync(id, item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpDelete("applications/{id}")]
    public async Task DeleteApplicationAsync(int id)
    {
        await _metaDataBl.DeleteApplicationAsync(id);
    }

    [HttpGet("applications")]
    public async Task<IEnumerable<ItemViewModel>> GetApplicationsAsync()
    {
        return await _metaDataBl.GetApplicationsAsync();
    }
    #endregion

    [Authorize(Roles.Administrator)]
    #region UnitAreas
    [HttpPost("unitareas")]
    public async Task<int> AddUnitAreaAsync([FromBody] Item item)
    {
        return await _metaDataBl.AddUnitAreaAsync(item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("unitareas/{id}")]
    public async Task UpdateUnitAreaAsync(int id, [FromBody] Item item)
    {
        await _metaDataBl.UpdateUnitAreaAsync(id, item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpDelete("unitareas/{id}")]
    public async Task DeleteUnitAreaAsync(int id)
    {
        await _metaDataBl.DeleteUnitAreaAsync(id);
    }

    [HttpGet("unitareas")]
    public async Task<IEnumerable<ItemViewModel>> GetUnitAreasAsync()
    {
        return await _metaDataBl.GetUnitAreasAsync();
    }
    #endregion


    #region Categories

    [Authorize(Roles.Administrator)]
    [HttpPost("categories")]
    public async Task<int> AddCategoryAsync([FromBody] CategoryItem item)
    {
        return await _metaDataBl.AddCategoryAsync(item.Name, item.IsFreeTier);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("categories/{id}")]
    public async Task UpdateCategoryAsync(int id, [FromBody] CategoryItem item)
    {
        await _metaDataBl.UpdateCategoryAsync(id, item.Name, item.IsFreeTier);
    }

    [Authorize(Roles.Administrator)]
    [HttpDelete("categories/{id}")]
    public async Task DeleteCategoryAsync(int id)
    {
        await _metaDataBl.DeleteCategoryAsync(id);
    }

    [HttpGet("categories")]
    public async Task<IEnumerable<CategoryItemViewModel>> GetCategoriesAsync()
    {
        return await _metaDataBl.GetCategoriesAsync();
    }

    #endregion

    [Authorize(Roles.Administrator)]
    #region Years
    [HttpPost("years")]
    public async Task<int> AddYearAsync([FromBody] Item item)
    {
        return await _metaDataBl.AddYearAsync(item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("years/{id}")]
    public async Task UpdateYearAsync(int id, [FromBody] Item item)
    {
        await _metaDataBl.UpdateYearAsync(id, item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpDelete("years/{id}")]
    public async Task DeleteYearAsync(int id)
    {
        await _metaDataBl.DeleteYearAsync(id);
    }

    [HttpGet("years")]
    public async Task<IEnumerable<ItemViewModel>> GetYearsAsync()
    {
        return await _metaDataBl.GetYearsAsync();
    }
    #endregion

    #region Sections
    [Authorize(Roles.Administrator)]
    [HttpPost("sections")]
    public async Task<int> AddSectionAsync([FromBody] Item item)
    {
        return await _metaDataBl.AddSectionAsync(item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("sections/{id}")]
    public async Task UpdateSectionAsync(int id, [FromBody] Item item)
    {
        await _metaDataBl.UpdateSectionAsync(id, item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpDelete("sections/{id}")]
    public async Task DeleteSectionAsync(int id)
    {
        await _metaDataBl.DeleteSectionAsync(id);
    }

    [HttpGet("sections")]
    public async Task<IEnumerable<ItemViewModel>> GetSectionsAsync()
    {
        return await _metaDataBl.GetSectionsAsync();
    }
    #endregion

    #region Courses
    [Authorize(Roles.Administrator)]
    [HttpPost("courses")]
    public async Task<int> AddCourseAsync([FromBody] Item item)
    {
        return await _metaDataBl.AddCourseAsync(item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("courses/{id}")]
    public async Task UpdateCourseAsync(int id, [FromBody] Item item)
    {
        await _metaDataBl.UpdateCourseAsync(id, item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpDelete("courses/{id}")]
    public async Task DeleteCourseAsync(int id)
    {
        await _metaDataBl.DeleteCourseAsync(id);
    }

    [HttpGet("courses")]
    public async Task<IEnumerable<ItemViewModel>> GetCoursesAsync()
    {
        return await _metaDataBl.GetCoursesAsync();
    }
    #endregion

    #region Colleges
    [Authorize(Roles.Administrator)]
    [HttpPost("colleges")]
    public async Task<int> AddCollegeAsync([FromBody] Item item)
    {
        return await _metaDataBl.AddCollegeAsync(item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpPut("colleges/{id}")]
    public async Task UpdateCollegeAsync(int id, [FromBody] Item item)
    {
        await _metaDataBl.UpdateCollegeAsync(id, item.Name);
    }

    [Authorize(Roles.Administrator)]
    [HttpDelete("colleges/{id}")]
    public async Task DeleteCollegeAsync(int id)
    {
        await _metaDataBl.DeleteCollegeAsync(id);
    }

    [HttpGet("colleges")]
    public async Task<IEnumerable<ItemViewModel>> GetCollegesAsync()
    {
        return await _metaDataBl.GetCollegesAsync();
    }
    #endregion
}