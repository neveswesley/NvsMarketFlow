namespace NvsMarketFlow.Application.Exceptions;

public class DuplicateFieldException : Exception
{
    public DuplicateFieldException(string entityName, string fieldName, string value)
        : base($"{entityName} with {fieldName} '{value}' already exists.")
    {
    }
}