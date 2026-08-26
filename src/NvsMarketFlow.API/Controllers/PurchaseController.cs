using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.Requests.Purchase;
using NvsMarketFlow.Application.UseCases.Purchase.Commands;
using NvsMarketFlow.Application.UseCases.Purchase.Queries;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PurchaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreatePurchaseRequest request, CancellationToken ct)
        {
            var command = new CreatePurchase.CreatePurchaseCommand(request);

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result
            );
        }
        
        [HttpPost("{purchaseId:guid}/items")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddItem(
            [FromRoute] Guid purchaseId,
            [FromBody] AddPurchaseItemRequest request,
            CancellationToken ct)
        {
            var command = new AddPurchaseItem.AddPurchaseItemCommand(purchaseId, request);

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.PurchaseId },
                result
            );
        }
        
        [HttpDelete("{purchaseId:guid}/items/{itemId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveItem(
            [FromRoute] Guid purchaseId,
            [FromRoute] Guid itemId,
            CancellationToken ct)
        {
            var command = new RemovePurchaseItem.RemovePurchaseItemCommand(purchaseId, itemId);

            await _mediator.Send(command, ct);

            return NoContent();
        }
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var query = new GetPurchaseById.GetPurchaseByIdQuery(id);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(
            CancellationToken ct,
            [FromQuery] Guid? supplierId,
            [FromQuery] string? invoiceNumber,
            [FromQuery] PurchaseStatus? status,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllPurchase.GetAllPurchaseQuery(
                supplierId, invoiceNumber, status, startDate, endDate, page, pageSize);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
        
        [HttpPatch("{id:guid}/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Confirm(
            [FromRoute] Guid id,
            [FromBody] ConfirmPurchaseRequest request,
            CancellationToken ct)
        {
            var command = new ConfirmPurchase.ConfirmPurchaseCommand(id, request);

            var result = await _mediator.Send(command, ct);

            return Ok(result);
        }
    }
}