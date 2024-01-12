using System.Collections.Generic;
using System.Threading.Tasks;
using IBSConnect.Business.Models;

namespace IBSConnect.Business;

public interface ISettingBL
{
    public Task<IEnumerable<SettingViewModel>> GetSettings();
    public Task UpdateSettingsAsync(IEnumerable<SettingViewModel> settings);

    public Task InitializePrevisousTransaction(string semesterName);
    public Task<int> ResetHistoryTransaction( IBSResetHistoryView history);
}