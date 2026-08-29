using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.Requests.CashMovement;
using NvsMarketFlow.Application.Requests.CashRegister;
using NvsMarketFlow.Application.UseCases.CashMovement.Commands;
using NvsMarketFlow.Application.UseCases.CashMovement.Queries;
using NvsMarketFlow.Application.UseCases.CashRegister.Commands;
using NvsMarketFlow.Application.UseCases.CashRegister.Queries;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CashRegisterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CashRegisterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("open")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Open([FromBody] OpenCashRegisterRequest request, CancellationToken ct)
        {
            var command = new OpenCashRegister.OpenCashRegisterCommand(request);

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result
            );
        }
        
        [HttpPatch("{id:guid}/close")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Close(
            [FromRoute] Guid id,
            [FromBody] CloseCashRegisterRequest request,
            CancellationToken ct)
        {
            var command = new CloseCashRegister.CloseCashRegisterCommand(id, request);

            var result = await _mediator.Send(command, ct);

            return Ok(result);
        }
        
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Administrator,Supervisor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var query = new GetCashRegisterById.GetCashRegisterByIdQuery(id);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrator,Supervisor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(
            CancellationToken ct,
            [FromQuery] Guid? userId,
            [FromQuery] CashRegisterStatus? status,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCashRegister.GetAllCashRegisterQuery(userId, status, startDate, endDate, page, pageSize);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
        
        [HttpPost("{cashRegisterId:guid}/movements")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMovement(
            [FromRoute] Guid cashRegisterId,
            [FromBody] CreateCashMovementRequest request,
            CancellationToken ct)
        {
            var command = new CreateCashMovement.CreateCashMovementCommand(cashRegisterId, request);

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.CashRegisterId },
                result
            );
        }

        [HttpGet("{cashRegisterId:guid}/movements")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllMovements(
            [FromRoute] Guid cashRegisterId,
            CancellationToken ct,
            [FromQuery] CashMovementType? type,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCashMovement.GetAllCashMovementQuery(cashRegisterId, type, page, pageSize);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
    }
}