using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Brand;
using NvsMarketFlow.Application.Responses.Brand;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Brand.Commands;

public class UpdateBrand
{
    public sealed record UpdateBrandCommand(Guid Id, UpdateBrandRequest Request) : IRequest<Unit>;

    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Unit>
    {
        private readonly IBrandReadOnlyRepository _brandReadOnlyRepository;
        private readonly IBrandWriteOnlyRepository _brandWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBrandCommandHandler(IBrandReadOnlyRepository brandReadOnlyRepository,
            IBrandWriteOnlyRepository brandWriteOnlyRepository, IUnitOfWork unitOfWork)
        {
            _brandReadOnlyRepository = brandReadOnlyRepository;
            _brandWriteOnlyRepository = brandWriteOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandReadOnlyRepository.GetByIdAsync(request.Id, cancellationToken);
            if (brand == null)
                throw new NotFoundException("Brand not found.");

            brand.Update(request.Request.Name);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}