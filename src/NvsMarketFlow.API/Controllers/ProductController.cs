using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.Requests.Product;
using NvsMarketFlow.Application.UseCases.Product.Commands;
using NvsMarketFlow.Application.UseCases.Product.Queries;
using NvsMarketFlow.Domain.Enums;

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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync(
            CancellationToken ct,
            [FromQuery] string? name,
            [FromQuery] Guid? categoryId,
            [FromQuery] Guid? brandId,
            [FromQuery] Status? status,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] bool? lowStock,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllProduct.GetAllProductQuery(name, categoryId, brandId, status, minPrice, maxPrice, lowStock, page, pageSize);
            
            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }

        [HttpGet("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid productId, CancellationToken ct)
        {
            var query = new GetProductById.GetProductByIdQuery(productId);
            
            var result = await _mediator.Send(query, ct);
            
            return Ok(result);
        }

        [HttpPut("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid productId, UpdateProductInfoRequest request, CancellationToken ct)
        {
            var command = new UpdateProduct.UpdateProductCommand(productId, request);

            await _mediator.Send(command, ct);
            
            return NoContent();
        }

        [HttpDelete("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid productId, CancellationToken ct)
        {
            var command = new DeleteProduct.DeleteProductCommand(productId);
            
            await _mediator.Send(command, ct);
            
            return NoContent();
        }
        
    }
}
