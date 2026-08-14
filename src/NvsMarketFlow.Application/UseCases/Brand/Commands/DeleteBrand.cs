using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Brand.Commands;

public class DeleteBrand
{
    public sealed record DeleteBrandCommand(Guid Id) : IRequest<Unit>;

    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Unit>
    {
        private readonly IBrandWriteOnlyRepository _brandWriteOnlyRepository;
        private readonly IBrandReadOnlyRepository _brandReadOnlyRepository;

        public DeleteBrandCommandHandler(IBrandWriteOnlyRepository brandWriteOnlyRepository, IBrandReadOnlyRepository brandReadOnlyRepository)
        {
            _brandWriteOnlyRepository = brandWriteOnlyRepository;
            _brandReadOnlyRepository = brandReadOnlyRepository;
        }

        public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandReadOnlyRepository.GetByIdAsync(request.Id, cancellationToken);

            if (brand == null)
                throw new NotFoundException("Brand not found.");
            
            await _brandWriteOnlyRepository.DeleteAsync(brand, cancellationToken);
            return Unit.Value;
        }
    }
}