using System.IO;

namespace IBSConnect.Business;

/// <summary>
/// A Wrapper around MemoryStream that can prevent the underlying stream from being closed during Workbook.Write
/// https://stackoverflow.com/questions/22931582/memorystream-seems-be-closed-after-npoi-workbook-write
/// </summary>
public class NpoiMemoryStream : MemoryStream
{
    public NpoiMemoryStream()
    {
    }

    /// <summary>
    /// Prevents the underlying <see cref="MemoryStream"/> from being closed. Set this to true before calling Workbook.Write
    /// and set to false immediately afterwards.
    /// </summary>
    public bool PreventClose { get; set; }

    public override void Close()
    {
        if (!PreventClose)
            base.Close();
    }
}