using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using IBSConnect.Business.Models;
using IBSConnect.Data;
using System;
using System.Data.Entity;
using MySqlX.XDevAPI.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySql.Data.MySqlClient;
using System.Data;
using IBSConnect.Data.Models;
using NPOI.SS.Formula.Functions;

namespace IBSConnect.Business;

public class SettingBL : ISettingBL
{
    private readonly IBSConnectDataContext _dataContext;

    public SettingBL(IBSConnectDataContext dataContext)
    {
        _dataContext = dataContext;
    }


    public async Task<IEnumerable<SettingViewModel>> GetSettings()
    {
        var result = _dataContext.Settings.ToList();
        var returnResult = new List<SettingViewModel>();
        foreach (var item in result)
        {
            var viewModel = new SettingViewModel();
            viewModel.Name = item.Name;
            viewModel.Value = item.Value;
            viewModel.Id = item.Id;
            returnResult.Add(viewModel);
        }
        return await Task.FromResult(returnResult);
    }

    public async Task UpdateSettingsAsync(IEnumerable<SettingViewModel> settings)
    {
        
        var Rate = string.Empty;
        var DefaultTime = string.Empty;
        var DefaultPassword = string.Empty;

        foreach (var setting in settings)
        {


            if (setting.Name == "Rate")
            {
                Rate = setting.Value;
                var entity = _dataContext.Settings.
                    Where(x => x.Name == setting.Name).FirstOrDefault();
                var result = _dataContext.Settings.FirstOrDefault(x => x.Id == entity.Id);
                if (result != null)
                {

                    result.Name = entity.Name;
                    result.Value = Rate;
                    _dataContext.Update(result);
                }


            }
            if (setting.Name == "DefaultTime")
            {
                DefaultTime = setting.Value;
                var entity = _dataContext.Settings.
                    Where(x => x.Name == setting.Name).FirstOrDefault();
                var result = _dataContext.Settings.FirstOrDefault(x => x.Id == entity.Id);
                if (result != null)
                {

                    result.Name = entity.Name;
                    result.Value = DefaultTime;
                    _dataContext.Update(result);
                }


            }
            if (setting.Name == "DefaultPassword")
            {
                DefaultPassword = setting.Value;
                var entity = _dataContext.Settings.
                    Where(x => x.Name == setting.Name).FirstOrDefault();
                var result = _dataContext.Settings.FirstOrDefault(x => x.Id == entity.Id);
                if (result != null)
                {

                    result.Name = entity.Name;
                    result.Value = DefaultPassword;
                    _dataContext.Update(result);
                }


            }

        }

        await _dataContext.SaveChangesAsync();
    }

    public async Task InitializePrevisousTransaction(string semesterName)
    {
        semesterName = "SY-2023-2024";
        //var result = await _dataContext.Database.ExecuteSqlInterpolatedAsync($"CALL `reset_alloted_time` ({semesterName})");
        //Console.WriteLine(result);


        MySqlConnection conn = new MySqlConnection();
        conn.ConnectionString = "server=localhost;database=ibs_connect;user=ibsuser;password=tz2YyXE?e&366sXJ";

        try
        {
            await conn.OpenAsync();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "EXEC RESET_ALLOTED_TIME; ";

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Sy_Semester", semesterName);
            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }

        conn.Close();






    }
    public async Task<int> ResetHistoryTransaction( IBSResetHistoryView history)
    {

        var dbHistory = new IBSResetHistory
        {
            UserId = history.UserId,
            Sy_Semester = history.Sy_Semester
        };


        await _dataContext.IBSResetHistories.AddAsync(dbHistory);

        await _dataContext.SaveChangesAsync();

        return dbHistory.Id;
    }

}