using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IBSConnect.Business.Models;

namespace IBSConnect.Business;

public interface IMemberSessionBL
{
    Task<DateTime> StartSessionAsync(int id, IEnumerable<int> applicationIds, int unitAreaId);
    Task<DateTime> EndSessionAsync(int id);
    Task<SessionViewModel> GetLastSessionAsync(int id);
    Task<CurrentSessionViewModel> GetCurrentSessionAsync(int id);
    Task<IEnumerable<SessionViewModel>> GetSessionsAsync(int id);
}