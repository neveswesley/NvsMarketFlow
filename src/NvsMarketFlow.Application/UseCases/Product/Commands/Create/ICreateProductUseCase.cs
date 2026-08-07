using NvsMarketFlow.Application.Requests.Product;
using NvsMarketFlow.Application.Responses.Product;

namespace NvsMarketFlow.Application.UseCases.Product.Commands.Create;

public interface ICreateProductUseCase
{
    Task<CreateProductResponse> ExecuteAsync(CreateProductRequest request);
}