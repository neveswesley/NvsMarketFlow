namespace NvsMarketFlow.Application.Exceptions;

public class CategoryHasLinkedProductsException : Exception
{
    public CategoryHasLinkedProductsException(string message) : base(message) { }
}