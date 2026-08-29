using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.Requests.Product;
using NvsMarketFlow.Application.UseCases.Product.Commands;
using NvsMarketFlow.Application.UseCases.Product.Queries;
using NvsMarketFlow.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync(CreateProductRequest request, CancellationToken ct)
        {
            var command = new CreateProduct.CreateProductCommand(request, ct);
            
            var result = await _mediator.Send(command, ct);
            
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result
            );
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(
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

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var query = new GetProductById.GetProductByIdQuery(id);
            
            var result = await _mediator.Send(query, ct);
            
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateProductInfoRequest request, CancellationToken ct)
        {
            var command = new UpdateProduct.UpdateProductCommand(id, request);

            await _mediator.Send(command, ct);
            
            return NoContent();
        }

        [HttpPatch("{id:guid}/activate")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Activate([FromRoute] Guid id, CancellationToken ct)
        {
            var command = new ActivateProduct.ActivateProductCommand(id);

            await _mediator.Send(command, ct);

            return NoContent();
        }

        [HttpPatch("{id:guid}/deactivate")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deactivate([FromRoute] Guid id, CancellationToken ct)
        {
            var command = new DeactivateProduct.DeactivateProductCommand(id);

            await _mediator.Send(command, ct);

            return NoContent();
        }
        
    }
}
