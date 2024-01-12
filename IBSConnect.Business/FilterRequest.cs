using System.Collections.Generic;

namespace IBSConnect.Business;

public class FilterRequest
{
    public string Query { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
    public string OrderBy { get; set; }
    public string SortOrder { get; set; }
}

public class BillingFilterRequest : FilterRequest
{
    public bool ShowAll { get; set; }
}