using System.Collections.Generic;

namespace IBSConnect.Business;

public class QueryResult<T>
{
    public int Count { get; set; }
    public IEnumerable<T> Result { get; set; }
    public Dictionary<string, IEnumerable<string>> ColumnValues { get; set; }
}