using System.Collections.Generic;
using System.Threading.Tasks;
using IBSConnect.Business.Models;

namespace IBSConnect.Business;

public interface IMetaDataBL
{
    Task<int> AddApplicationAsync(string name);
    Task UpdateApplicationAsync(int id, string name);
    Task DeleteApplicationAsync(int id);
    Task<IEnumerable<ItemViewModel>> GetApplicationsAsync();
    Task<int> AddUnitAreaAsync(string name);
    Task UpdateUnitAreaAsync(int id, string name);
    Task DeleteUnitAreaAsync(int id);
    Task<IEnumerable<ItemViewModel>> GetUnitAreasAsync();
    Task<int> AddCategoryAsync(string name, bool isFreeTier);
    Task UpdateCategoryAsync(int id, string name, bool isFreeTier);
    Task DeleteCategoryAsync(int id);
    Task<IEnumerable<CategoryItemViewModel>> GetCategoriesAsync();
    Task<int> AddYearAsync(string name);
    Task UpdateYearAsync(int id, string name);
    Task DeleteYearAsync(int id);
    Task<IEnumerable<ItemViewModel>> GetYearsAsync();
    Task<int> AddCourseAsync(string name);
    Task UpdateCourseAsync(int id, string name);
    Task DeleteCourseAsync(int id);
    Task<IEnumerable<ItemViewModel>> GetCoursesAsync();
    Task<int> AddSectionAsync(string name);
    Task UpdateSectionAsync(int id, string name);
    Task DeleteSectionAsync(int id);
    Task<IEnumerable<ItemViewModel>> GetSectionsAsync();
    Task<int> AddCollegeAsync(string name);
    Task UpdateCollegeAsync(int id, string name);
    Task DeleteCollegeAsync(int id);
    Task<IEnumerable<ItemViewModel>> GetCollegesAsync();
}