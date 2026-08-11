namespace NvsMarketFlow.Application.Exceptions;

public class DuplicateProductSkuException : Exception
{
    public DuplicateProductSkuException(string message) : base(message) { }
}