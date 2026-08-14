using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.Requests.Brand;
using NvsMarketFlow.Application.UseCases.Brand.Commands;
using NvsMarketFlow.Application.UseCases.Brand.Queries;

namespace NvsMarketFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateBrandRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateBrand.CreateBrandCommand(request);

            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result
            );
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var query = new GetBrandById.GetBrandByIdQuery(id);
            
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);

        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken,
            [FromQuery] string? name,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllBrands.GetAllBrandsQuery(name, page, pageSize);
            
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteBrand.DeleteBrandCommand(id);
            
            await _mediator.Send(command, cancellationToken);
            
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBrandRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateBrand.UpdateBrandCommand(id, request);
            
            await _mediator.Send(command, cancellationToken);
            
            return NoContent();
            
        }
        
    }
}
