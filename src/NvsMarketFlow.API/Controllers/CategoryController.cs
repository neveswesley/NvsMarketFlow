using MediatR;
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken ct)
        {
            var command = new CreateCategory.CreateCategoryCommand(request);
            
            var result = await _mediator.Send(command, ct);
            
            return Created(string.Empty, result);
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

        [HttpGet("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid categoryId, CancellationToken ct)
        {
            var query = new GetCategoryById.GetByIdQuery(categoryId);
            return Ok(await _mediator.Send(query, ct));
        }
        
        [HttpPut("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] Guid categoryId, [FromBody] UpdateCategoryRequest request, CancellationToken ct)
        {
            var command = new UpdateCategory.UpdateCategoryCommand(categoryId, request);
            
            await _mediator.Send(command, ct);
            
            return NoContent();
        }

        [HttpDelete("{categoryId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete([FromRoute] Guid categoryId, CancellationToken ct)
        {
            var command = new DeleteCategory.DeleteCategoryCommand(categoryId);

            await _mediator.Send(command, ct);
            
            return NoContent();
        }
    }
}
