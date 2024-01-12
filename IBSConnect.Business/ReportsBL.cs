using System.IO;
using System.Threading.Tasks;
using IBSConnect.Business.Models;
using IBSConnect.Data;
using System.Linq;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace IBSConnect.Business;

public class ReportsBL : IReportsBL
{
    private readonly IBSConnectDataContext _dataContext;

    public ReportsBL(IBSConnectDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Stream> GetOutstandingBillsReportAsync()
    {
        var rate = await GetRate();

        var outstandingBills = await _dataContext.MemberBills.Where(m => m.ExcessMinutes > 0).ToListAsync();

        IWorkbook workbook = new XSSFWorkbook();

        ISheet sheet = workbook.CreateSheet("Sheet1");

        var titleStyle = workbook.CreateCellStyle();
        var titleFont = workbook.CreateFont();
        titleFont.IsBold = true;
        titleFont.FontHeightInPoints = 24;
        titleStyle.SetFont(titleFont);

        var styleCurrency = workbook.CreateCellStyle();
        styleCurrency.DataFormat = workbook.CreateDataFormat().GetFormat("\"Php\"#,##0.00");

        //var styleDate = workbook.CreateCellStyle();
        //styleDate.DataFormat = workbook.CreateDataFormat().GetFormat("MM/dd/yyyy");

        var headerStyle = workbook.CreateCellStyle();
        var font = workbook.CreateFont();
        font.IsBold = true;
        headerStyle.SetFont(font);

        IRow row;
        var rownum = 0;
        row = sheet.CreateRow(rownum);

        row.MakeCell(0, "I.D. No", headerStyle);
        row.MakeCell(1, "Name", headerStyle);
        row.MakeCell(2, "Billable Minutes", headerStyle);
        row.MakeCell(3, "Payable Amount", headerStyle);
        rownum++;


        foreach (var bill in outstandingBills)
        {
            var colnum = 0;
            row = sheet.CreateRow(rownum++);
            row.MakeCell(colnum++, bill.IdNo);
            row.MakeCell(colnum++, bill.LastName + ", " + bill.FirstName + " " + bill.MiddleName);
            row.MakeCell(colnum++, bill.ExcessMinutes);
            row.MakeCell(colnum++, (double)(bill.ExcessMinutes * rate), styleCurrency);
        }


        var stream = new NpoiMemoryStream();
        stream.PreventClose = true;
        workbook.Write(stream);
        stream.Flush();
        stream.Seek(0, SeekOrigin.Begin);
        stream.PreventClose = false;

        return stream;


    }

    public async Task<Stream> UsageByCollegeAsync(DateRangeRequest request)
    {
        var connectionString = _dataContext.Database.GetConnectionString();

        var sql = @"
        select CollegeId, Name, sum(TIMESTAMPDIFF(MINUTE, ms.StartTime, ms.EndTime)) as `Usage` from member_sessions ms
            inner
        join members m on ms.MemberId = m.Id
        inner
            join colleges c on m.CollegeId = c.Id
        where ms.EndTime is not null
            and ms.StartTime >= @Start and ms.StartTime <= @End
        group by CollegeId, Name
";

        await using var connection = new MySqlConnection(connectionString);

        await connection.OpenAsync();

        var start = request.Start.Date.ToString("yyyy-MM-dd") + " 00:00:00";
        var end = request.End.Date.ToString("yyyy-MM-dd") + " 23:59:59";
        var usages = await connection.QueryAsync<UsageByCollege>(sql, new { Start = start, end = end });

        IWorkbook workbook = new XSSFWorkbook();

        ISheet sheet = workbook.CreateSheet("Sheet1");

        var titleStyle = workbook.CreateCellStyle();
        var titleFont = workbook.CreateFont();
        titleFont.IsBold = true;
        titleFont.FontHeightInPoints = 24;
        titleStyle.SetFont(titleFont);

        //var styleDate = workbook.CreateCellStyle();
        //styleDate.DataFormat = workbook.CreateDataFormat().GetFormat("MM/dd/yyyy");

        var headerStyle = workbook.CreateCellStyle();
        var font = workbook.CreateFont();
        font.IsBold = true;
        headerStyle.SetFont(font);

        IRow row;
        var rownum = 0;

        row = sheet.CreateRow(rownum);
        row.MakeCell(0, $"Usage by School - {request.Start.Date:d} to {request.End.Date:d}", titleStyle);
        rownum++;

        row = sheet.CreateRow(rownum);

        row.MakeCell(0, "School", headerStyle);
        row.MakeCell(1, "Minutes Used", headerStyle);
        rownum++;


        foreach (var usage in usages)
        {
            var colnum = 0;
            row = sheet.CreateRow(rownum++);
            row.MakeCell(colnum++, usage.Name);
            row.MakeCell(colnum++, usage.Usage);
        }


        var stream = new NpoiMemoryStream();
        stream.PreventClose = true;
        workbook.Write(stream);
        stream.Flush();
        stream.Seek(0, SeekOrigin.Begin);
        stream.PreventClose = false;

        return stream;
    }

    public async Task<Stream> UsageByUnitAreaAsync(DateRangeRequest request)
    {
        var connectionString = _dataContext.Database.GetConnectionString();

        var sql = @"
        select u.Name as UnitArea, cat.Name as Category, col.Name as College, crs.Name as Course, yr.Name as Year, COUNT(m.Id) as `Count` from member_sessions ms
        inner
            join members m on ms.MemberId = m.Id
        inner
            join unit_areas u on ms.UnitAreaId = u.Id
        inner
            join categories cat on m.CategoryId = cat.Id
        inner
            join colleges col on m.CollegeId = col.Id
        inner
            join courses crs on m.CourseId = crs.Id
        inner
            join years yr on m.YearId = yr.Id
        where ms.EndTime is not null
            and ms.StartTime >= @Start and ms.StartTime <= @End
        group by u.Name, cat.Name, col.Name, crs.Name, yr.Name
        order by u.Name, cat.Name, col.Name, crs.Name, yr.Name
";

        await using var connection = new MySqlConnection(connectionString);

        await connection.OpenAsync();

        var start = request.Start.Date.ToString("yyyy-MM-dd") + " 00:00:00";
        var end = request.End.Date.ToString("yyyy-MM-dd") + " 23:59:59";
        var usages = await connection.QueryAsync<UsageByDemographicUnitArea>(sql, new { Start = start, end = end });

        IWorkbook workbook = new XSSFWorkbook();

        ISheet sheet = workbook.CreateSheet("Sheet1");

        var titleStyle = workbook.CreateCellStyle();
        var titleFont = workbook.CreateFont();
        titleFont.IsBold = true;
        titleFont.FontHeightInPoints = 24;
        titleStyle.SetFont(titleFont);

        //var styleDate = workbook.CreateCellStyle();
        //styleDate.DataFormat = workbook.CreateDataFormat().GetFormat("MM/dd/yyyy");

        var headerStyle = workbook.CreateCellStyle();
        var font = workbook.CreateFont();
        font.IsBold = true;
        headerStyle.SetFont(font);

        IRow row;
        var rownum = 0;
        row = sheet.CreateRow(rownum);
        row.MakeCell(0, $"Usage by Unit Area - {request.Start.Date:d} to {request.End.Date:d}", titleStyle);
        rownum++;

        row = sheet.CreateRow(rownum);

        row.MakeCell(0, "Unit Area", headerStyle);
        row.MakeCell(1, "Category", headerStyle);
        row.MakeCell(2, "School", headerStyle);
        row.MakeCell(3, "Course", headerStyle);
        row.MakeCell(4, "Year", headerStyle);
        row.MakeCell(5, "Count", headerStyle);
        rownum++;

        string lastUnitArea = null;
        bool newGroup;

        foreach (var usage in usages)
        {
            var colnum = 0;
            newGroup = false;

            if (lastUnitArea != usage.UnitArea)
            {
                if (lastUnitArea != null)
                {
                    rownum++;
                }

                newGroup = true;
            }

            row = sheet.CreateRow(rownum++);

            if (newGroup)
            {
                row.MakeCell(colnum++, usage.UnitArea);
            }
            else
            {
                colnum++;
            }

            row.MakeCell(colnum++, usage.Category);
            row.MakeCell(colnum++, usage.College);
            row.MakeCell(colnum++, usage.Course);
            row.MakeCell(colnum++, usage.Year);
            row.MakeCell(colnum++, usage.Count);
            lastUnitArea = usage.UnitArea;
        }

        var stream = new NpoiMemoryStream();
        stream.PreventClose = true;
        workbook.Write(stream);
        stream.Flush();
        stream.Seek(0, SeekOrigin.Begin);
        stream.PreventClose = false;

        return stream;
    }

    public async Task<Stream> UsageByDemographicsAsync(DateRangeRequest request)
    {
        var connectionString = _dataContext.Database.GetConnectionString();

        var sql = @"
        select cat.Name as Category, col.Name as College, crs.Name as Course, yr.Name as Year, COUNT(m.Id) as `Count` from member_sessions ms
            inner
        join members m on ms.MemberId = m.Id
        inner
            join categories cat on m.CategoryId = cat.Id
        inner
            join colleges col on m.CollegeId = col.Id
        inner
            join courses crs on m.CourseId = crs.Id
        inner
            join years yr on m.YearId = yr.Id
        where ms.EndTime is not null
            and ms.StartTime >= @Start and ms.StartTime <= @End
        group by cat.Name, col.Name, crs.Name, yr.Name
        order by cat.Name, col.Name, crs.Name, yr.Name
";

        await using var connection = new MySqlConnection(connectionString);

        await connection.OpenAsync();

        var start = request.Start.Date.ToString("yyyy-MM-dd") + " 00:00:00";
        var end = request.End.Date.ToString("yyyy-MM-dd") + " 23:59:59";
        var usages = await connection.QueryAsync<UsageByDemographics>(sql, new { Start = start, end = end });

        IWorkbook workbook = new XSSFWorkbook();

        ISheet sheet = workbook.CreateSheet("Sheet1");

        var titleStyle = workbook.CreateCellStyle();
        var titleFont = workbook.CreateFont();
        titleFont.IsBold = true;
        titleFont.FontHeightInPoints = 24;
        titleStyle.SetFont(titleFont);

        //var styleDate = workbook.CreateCellStyle();
        //styleDate.DataFormat = workbook.CreateDataFormat().GetFormat("MM/dd/yyyy");

        var headerStyle = workbook.CreateCellStyle();
        var font = workbook.CreateFont();
        font.IsBold = true;
        headerStyle.SetFont(font);

        IRow row;
        var rownum = 0;
        row = sheet.CreateRow(rownum);
        row.MakeCell(0, $"Usage by Demographic - {request.Start.Date:d} to {request.End.Date:d}", titleStyle);
        rownum++;

        row = sheet.CreateRow(rownum);

        row.MakeCell(0, "Category", headerStyle);
        row.MakeCell(1, "School", headerStyle);
        row.MakeCell(2, "Course", headerStyle);
        row.MakeCell(3, "Year", headerStyle);
        row.MakeCell(4, "Count", headerStyle);
        rownum++;

        string lastCategory = null;
        bool newGroup = false;

        foreach (var usage in usages)
        {
            var colnum = 0;

            if (lastCategory != usage.Category)
            {
                if (lastCategory != null)
                {
                    rownum++;
                }

                newGroup = true;
            }

            row = sheet.CreateRow(rownum++);

            if (newGroup)
            {
                row.MakeCell(colnum++, usage.Category);
            }
            else
            {
                colnum++;
            }
            row.MakeCell(colnum++, usage.College);
            row.MakeCell(colnum++, usage.Course);
            row.MakeCell(colnum++, usage.Year);
            row.MakeCell(colnum++, usage.Count);
            lastCategory = usage.Category;
        }

        var stream = new NpoiMemoryStream();
        stream.PreventClose = true;
        workbook.Write(stream);
        stream.Flush();
        stream.Seek(0, SeekOrigin.Begin);
        stream.PreventClose = false;

        return stream;
    }


    public async Task<Stream> UsageByDemographicApplicationsAsync(DateRangeRequest request)
    {
        var connectionString = _dataContext.Database.GetConnectionString();

        var sql = @"
        select a.Name as Application, cat.Name as Category, col.Name as College, crs.Name as Course, yr.Name as Year, COUNT(m.Id) as `Count` from member_sessions ms
        inner
            join members m on ms.MemberId = m.Id
        inner
            join session_applications s on ms.Id = s.SessionId
        inner
            join applications a on s.ApplicationId = a.Id
        inner
            join categories cat on m.CategoryId = cat.Id
        inner
            join colleges col on m.CollegeId = col.Id
        inner
            join courses crs on m.CourseId = crs.Id
        inner
            join years yr on m.YearId = yr.Id
        where ms.EndTime is not null
            and ms.StartTime >= @Start and ms.StartTime <= @End
        group by a.Name, cat.Name, col.Name, crs.Name, yr.Name
        order by a.Name, cat.Name, col.Name, crs.Name, yr.Name
";

        await using var connection = new MySqlConnection(connectionString);

        await connection.OpenAsync();

        var start = request.Start.Date.ToString("yyyy-MM-dd") + " 00:00:00";
        var end = request.End.Date.ToString("yyyy-MM-dd") + " 23:59:59";
        var usages = await connection.QueryAsync<UsageByDemographicApplication>(sql, new { Start = start, end = end });

        IWorkbook workbook = new XSSFWorkbook();

        ISheet sheet = workbook.CreateSheet("Sheet1");

        var titleStyle = workbook.CreateCellStyle();
        var titleFont = workbook.CreateFont();
        titleFont.IsBold = true;
        titleFont.FontHeightInPoints = 24;
        titleStyle.SetFont(titleFont);

        //var styleDate = workbook.CreateCellStyle();
        //styleDate.DataFormat = workbook.CreateDataFormat().GetFormat("MM/dd/yyyy");

        var headerStyle = workbook.CreateCellStyle();
        var font = workbook.CreateFont();
        font.IsBold = true;
        headerStyle.SetFont(font);

        IRow row;
        var rownum = 0;
        row = sheet.CreateRow(rownum);
        row.MakeCell(0, $"Usage by Application - {request.Start.Date:d} to {request.End.Date:d}", titleStyle);
        rownum++;

        row = sheet.CreateRow(rownum);

        row.MakeCell(0, "Application", headerStyle);
        row.MakeCell(1, "Category", headerStyle);
        row.MakeCell(2, "School", headerStyle);
        row.MakeCell(3, "Course", headerStyle);
        row.MakeCell(4, "Year", headerStyle);
        row.MakeCell(5, "Count", headerStyle);
        rownum++;

        string lastApplication = null;
        bool newGroup;

        foreach (var usage in usages)
        {
            var colnum = 0;
            newGroup = false;

            if (lastApplication != usage.Application)
            {
                if (lastApplication != null)
                {
                    rownum++;
                }

                newGroup = true;
            }

            row = sheet.CreateRow(rownum++);

            if (newGroup)
            {
                row.MakeCell(colnum++, usage.Application);
            }
            else
            {
                colnum++;
            }

            row.MakeCell(colnum++, usage.Category);
            row.MakeCell(colnum++, usage.College);
            row.MakeCell(colnum++, usage.Course);
            row.MakeCell(colnum++, usage.Year);
            row.MakeCell(colnum++, usage.Count);
            lastApplication = usage.Application;
        }

        var stream = new NpoiMemoryStream();
        stream.PreventClose = true;
        workbook.Write(stream);
        stream.Flush();
        stream.Seek(0, SeekOrigin.Begin);
        stream.PreventClose = false;

        return stream;
    }


    private async Task<decimal> GetRate()
    {
        return await _dataContext.Settings.Where(s => s.Name == "Rate").Select(s => decimal.Parse(s.Value)).SingleAsync();
    }

}