using MediatR;
using NvsMarketFlow.Application.Responses.Category;

namespace NvsMarketFlow.Application.UseCases.Category.Queries;

public sealed record GetAllQuery() : IRequest<GetAllCategoryResponse>
{
    
}