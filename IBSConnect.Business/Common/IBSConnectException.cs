using System;

namespace IBSConnect.Business.Common;

public class IBSConnectException : Exception
{
    public IBSConnectException(string message) : base(message)
    {
    }
}