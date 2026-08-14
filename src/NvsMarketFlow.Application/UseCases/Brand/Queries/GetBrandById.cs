using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.Brand;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Brand.Queries;

public class GetBrandById
{
    public sealed record GetBrandByIdQuery(Guid Id) : IRequest<GetBrandResponse>;

    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, GetBrandResponse>
    {
        private readonly IBrandReadOnlyRepository _brandReadOnlyRepository;

        public GetBrandByIdQueryHandler(IBrandReadOnlyRepository brandReadOnlyRepository)
        {
            _brandReadOnlyRepository = brandReadOnlyRepository;
        }

        public async Task<GetBrandResponse> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var brand = await _brandReadOnlyRepository.GetByIdAsync(request.Id, cancellationToken);
            if (brand == null)
                throw new NotFoundException("Brand not found");

            return new GetBrandResponse()
            {
                Name = brand.Name
            };
        }
    }
}