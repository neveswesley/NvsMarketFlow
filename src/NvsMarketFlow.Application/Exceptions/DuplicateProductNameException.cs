namespace NvsMarketFlow.Application.Exceptions;

public class DuplicateProductNameException : Exception
{
    public DuplicateProductNameException(string message) : base(message) { }
}