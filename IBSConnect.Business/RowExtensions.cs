using System;
using NPOI.SS.UserModel;

namespace IBSConnect.Business;

public static class RowExtensions
{
    public static ICell MakeCellFormula(this IRow row, int colnum, string formula, ICellStyle cellStyle = null)
    {
        var cell = row.CreateCell(colnum);

        cell.SetCellType(CellType.Formula);

        cell.SetCellFormula(formula);

        if (cellStyle != null)
        {
            cell.CellStyle = cellStyle;
        }

        return cell;
    }

    public static ICell MakeCell(this IRow row, int colnum, string value, ICellStyle cellStyle = null)
    {
        var cell = row.CreateCell(colnum);

        cell.SetCellValue(value);

        if (cellStyle != null)
        {
            cell.CellStyle = cellStyle;
        }

        return cell;
    }

    public static ICell MakeCell(this IRow row, int colnum, DateTime value, ICellStyle cellStyle = null)
    {
        var cell = row.CreateCell(colnum);

        cell.SetCellValue(value);

        if (cellStyle != null)
        {
            cell.CellStyle = cellStyle;
        }

        return cell;
    }

    public static ICell MakeCell(this IRow row, int colnum, IRichTextString value, ICellStyle cellStyle = null)
    {
        var cell = row.CreateCell(colnum);

        cell.SetCellValue(value);

        if (cellStyle != null)
        {
            cell.CellStyle = cellStyle;
        }

        return cell;
    }

    public static ICell MakeCell(this IRow row, int colnum, bool value, ICellStyle cellStyle = null)
    {
        var cell = row.CreateCell(colnum);

        cell.SetCellValue(value);

        if (cellStyle != null)
        {
            cell.CellStyle = cellStyle;
        }

        return cell;
    }


    public static ICell MakeCell(this IRow row, int colnum, double value, ICellStyle cellStyle = null)
    {
        var cell = row.CreateCell(colnum);

        cell.SetCellValue(value);

        if (cellStyle != null)
        {
            cell.CellStyle = cellStyle;
        }

        return cell;
    }
}