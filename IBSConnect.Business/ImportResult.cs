using System.Collections.Generic;

namespace IBSConnect.Business;

public class ImportResult
{
    public int Updated { get; set; }
    public int Added { get; set; }
    public IEnumerable<string> Errors { get; set; }
}