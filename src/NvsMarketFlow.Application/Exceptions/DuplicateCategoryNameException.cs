namespace NvsMarketFlow.Application.Exceptions;

public class DuplicateCategoryNameException : Exception
{
    public DuplicateCategoryNameException(string message) : base(message) { }
}