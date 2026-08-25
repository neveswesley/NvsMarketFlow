using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Brand;
using NvsMarketFlow.Application.Responses.Brand;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Brand.Commands;

public class CreateBrand
{
    public sealed record CreateBrandCommand(CreateBrandRequest Request) : IRequest<CreateBrandResponse>;

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreateBrandResponse>
    {
        private readonly IBrandWriteOnlyRepository _brandWriteOnlyRepository;
        private readonly IBrandReadOnlyRepository _brandReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBrandCommandHandler(IBrandWriteOnlyRepository brandWriteOnlyRepository,
            IBrandReadOnlyRepository brandReadOnlyRepository, IUnitOfWork unitOfWork)
        {
            _brandWriteOnlyRepository = brandWriteOnlyRepository;
            _brandReadOnlyRepository = brandReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateBrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var existingName =
                await _brandReadOnlyRepository.ExistsByNameAsync(request.Request.Name, cancellationToken);

            if (existingName)
                throw new DuplicateFieldException("Brand", "name", request.Request.Name);

            var brand = new Domain.Entities.Brand(request.Request.Name);

            await _brandWriteOnlyRepository.CreateAsync(brand, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateBrandResponse()
            {
                Id = brand.Id,
                Name = brand.Name
            };
        }
    }
}