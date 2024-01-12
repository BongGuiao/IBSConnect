using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IBSConnect.Data.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace IBSConnect.Business;

public class ExcelReader
{
    private readonly IMetaDataBL _metaDataBl;

    public IEnumerable<ImportError> Errors { get; private set; }

    public ExcelReader(IMetaDataBL metaDataBl)
    {
        _metaDataBl = metaDataBl;
    }

    public async Task<IEnumerable<Member>> ReadAsync(Stream stream)
    {
        var errors = new List<ImportError>();

        var categories = await _metaDataBl.GetCategoriesAsync();
        var colleges = await _metaDataBl.GetCollegesAsync();
        var courses = await _metaDataBl.GetCoursesAsync();
        var years = await _metaDataBl.GetYearsAsync();

        var comparer = new CaseInsensitiveEqualityComparer();

        var categoriesLookup = categories.ToDictionary(d => d.Name, d => d.Id, comparer);
        var collegesLookup = colleges.ToDictionary(d => d.Name, d => d.Id, comparer);
        var coursesLookup = courses.ToDictionary(d => d.Name, d => d.Id, comparer);
        var yearsLookup = years.ToDictionary(d => d.Name, d => d.Id, comparer);

        var result = new List<Member>();

        stream.Position = 0;
        XSSFWorkbook xssWorkbook = new XSSFWorkbook(stream);

        var sheet = xssWorkbook.GetSheetAt(0);

        IRow headerRow = sheet.GetRow(0);
        int cellCount = headerRow.LastCellNum;

        //for (int j = 0; j < cellCount; j++)
        //{
        //    ICell cell = headerRow.GetCell(j);
        //    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
        //    {

        //    }
        //}
        void TryLookup(ICell cell, int row, int col, Dictionary<string, int> lookup, Action<int> onSuccess, string errorMessage)
        {
            {
                var value = "";

                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        value = cell.NumericCellValue.ToString();
                        break;
                    case CellType.String:
                        value = cell.StringCellValue.Trim();
                        break;
                    default:
                        errors.Add(new ImportError()
                        {
                            Row = row,
                            Column = col,
                            Message = errorMessage.Replace("{0}", value)
                        });
                        return;
                }

                if (lookup.TryGetValue(value, out var id))
                {
                    onSuccess(id);
                }
                else
                {
                    errors.Add(new ImportError()
                    {
                        Row = row,
                        Column = col,
                        Message = errorMessage.Replace("{0}", value)
                    });
                }
            }
        }


        int i = 0;
        int j = 0;
        for (i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
        {
            try
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                var member = new Member();

                for (j = row.FirstCellNum; j < cellCount; j++)
                {
                    var cell = row.GetCell(j);

                    if (cell != null)
                    {
                        switch (j)
                        {
                            case 0:
                                if (cell.CellType == CellType.Numeric)
                                {
                                    member.IdNo = cell.NumericCellValue.ToString();
                                }
                                else if (cell.CellType == CellType.String)
                                {
                                    member.IdNo = cell.StringCellValue;
                                }
                                break;
                            case 1:
                                member.LastName = cell.StringCellValue;
                                break;
                            case 2:
                                member.FirstName = cell.StringCellValue;
                                break;
                            case 3:
                                member.MiddleName = cell.StringCellValue;
                                break;
                            case 4:
                                member.Age = (int)cell.NumericCellValue;
                                break;
                            case 5:
                                TryLookup(cell, i, j, categoriesLookup, id => member.CategoryId = id, "No matching Category found: {0}");
                                break;
                            case 6:
                                TryLookup(cell, i, j, collegesLookup, id => member.CollegeId = id, "No matching College found: {0}");
                                break;
                            case 7:
                                TryLookup(cell, i, j, coursesLookup, id => member.CourseId = id, "No matching Course found: {0}");
                                break;
                            case 8:
                                // Year
                                TryLookup(cell, i, j, yearsLookup, id => member.YearId = id, "No matching Year found: {0}");
                                break;
                            case 9:
                                member.Section = cell.CellType switch
                                {
                                    // section
                                    CellType.Numeric => cell.NumericCellValue.ToString(),
                                    CellType.String => cell.StringCellValue,
                                    _ => member.Section
                                };
                                break;
                            case 10:
                                member.Notes = cell.StringCellValue;
                                break;
                        }

                        //if (!string.IsNullOrEmpty(cell.ToString()) && !string.IsNullOrWhiteSpace(cell.ToString()))
                        //{
                        //}
                    }



                }

                result.Add(member);

                //if (rowList.Count > 0)
                //    dtTable.Rows.Add(rowList.ToArray());
                //rowList.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        Errors = errors;

        return result;
    }
}