using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.Requests.Category;
using NvsMarketFlow.Application.UseCases.Category.Commands;
using NvsMarketFlow.Application.UseCases.Category.Queries;

namespace NvsMarketFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken ct)
        {
            var command = new CreateCategory.CreateCategoryCommand(request);
    
            var result = await _mediator.Send(command, ct);
    
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result
            );
        }

        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? name,
            CancellationToken ct,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCategory.GetAllCategoriesQuery(name, page, pageSize);

            return Ok(await _mediator.Send(query, ct));
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var query = new GetCategoryById.GetByIdQuery(id);
            return Ok(await _mediator.Send(query, ct));
        }
        
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken ct)
        {
            var command = new UpdateCategory.UpdateCategoryCommand(id, request);
            
            await _mediator.Send(command, ct);
            
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var command = new DeleteCategory.DeleteCategoryCommand(id);

            await _mediator.Send(command, ct);
            
            return NoContent();
        }
    }
}
