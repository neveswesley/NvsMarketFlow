using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.Requests.Product;
using NvsMarketFlow.Application.UseCases.Product.Commands;

namespace NvsMarketFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync(CreateProductRequest request, CancellationToken ct)
        {
            var command = new CreateProduct.CreateProductCommand(request, ct);
            var result = await _mediator.Send(command, ct);
            return Created(string.Empty, result);
        }
        
    }
}
