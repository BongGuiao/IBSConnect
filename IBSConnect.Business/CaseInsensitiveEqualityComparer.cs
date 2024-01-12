using System;
using System.Collections.Generic;

namespace IBSConnect.Business;

public class CaseInsensitiveEqualityComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        if (x == null || y == null)
        {
            return false;
        }
        return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
    }

    public int GetHashCode(string obj)
    {
        return obj.ToLowerInvariant().GetHashCode();
    }
}