using System;
using System.Collections.Generic;

namespace IBSConnect.Business.Common;

public class ValidationException : Exception
{
    public IEnumerable<string> Messages { get; set; }
    public ValidationException(IEnumerable<string> messages) : base("Validation failed")
    {
        Messages = messages;
    }
}