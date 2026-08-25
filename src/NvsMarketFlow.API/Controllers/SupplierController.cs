using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NvsMarketFlow.Application.Requests.Supplier;
using NvsMarketFlow.Application.UseCases.Supplier.Commands;
using NvsMarketFlow.Application.UseCases.Supplier.Queries;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupplierController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSupplierRequest request, CancellationToken ct)
        {
            var command = new CreateSupplier.CreateSupplierCommand(request);

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
            [FromQuery] string? corporateName,
            [FromQuery] string? fantasyName,
            [FromQuery] string? cnpj,
            [FromQuery] Status? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllSupplier.GetAllSupplierQuery(corporateName, fantasyName, cnpj, status, page, pageSize);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var query = new GetSupplierById.GetSupplierByIdQuery(id);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
        
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateSupplierInfoRequest request, CancellationToken ct)
        {
            var command = new UpdateSupplier.UpdateSupplierCommand(id, request);

            await _mediator.Send(command, ct);

            return NoContent();
        }

        [HttpPatch("{id:guid}/activate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Activate([FromRoute] Guid id, CancellationToken ct)
        {
            var command = new ActivateSupplier.ActivateSupplierCommand(id);

            await _mediator.Send(command, ct);

            return NoContent();
        }

        [HttpPatch("{id:guid}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Deactivate([FromRoute] Guid id, CancellationToken ct)
        {
            var command = new DeactivateSupplier.DeactivateSupplierCommand(id);

            await _mediator.Send(command, ct);

            return NoContent();
        }
    }
}
