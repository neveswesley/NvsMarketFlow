using FluentValidation;
using NvsMarketFlow.Application.UseCases.Product.Queries;

namespace NvsMarketFlow.Application.UseCases.Product.Validators;

public sealed class GetProductByBarcodeQueryValidator
    : AbstractValidator<GetProductByBarcode.GetProductByBarcodeQuery>
{
    public GetProductByBarcodeQueryValidator()
    {
        RuleFor(x => x.Barcode)
            .NotEmpty().WithMessage("Barcode is required.");
    }
}