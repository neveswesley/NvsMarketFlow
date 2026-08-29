using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.Requests.StockMovement;
using NvsMarketFlow.Application.UseCases.StockMovement.Commands;
using NvsMarketFlow.Application.UseCases.StockMovement.Queries;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator,Supervisor")]
    public class StockMovementController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        public StockMovementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateStockMovementRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateStockMovement.CreateStockMovementCommand(request);
    
            var result = await _mediator.Send(command, cancellationToken);

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
            [FromQuery] Guid? productId,
            [FromQuery] Guid? userId,
            [FromQuery] MovementType? movementType,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllStockMovement.GetAllStockMovementQuery(
                productId, userId, movementType, startDate, endDate, page, pageSize);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var query = new GetStockMovementById.GetStockMovementByIdQuery(id);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
        
    }
}
