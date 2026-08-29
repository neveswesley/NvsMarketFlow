using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.Requests.Sale;
using NvsMarketFlow.Application.UseCases.Sale.Commands;
using NvsMarketFlow.Application.UseCases.Sale.Queries;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSaleRequest request, CancellationToken ct)
        {
            var command = new CreateSale.CreateSaleCommand(request);

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result
            );
        }
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var query = new GetSaleById.GetSaleByIdQuery(id);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
        
        [HttpPost("{saleId:guid}/items")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddItem(
            [FromRoute] Guid saleId,
            [FromBody] AddSaleItemRequest request,
            CancellationToken ct)
        {
            var command = new AddSaleItem.AddSaleItemCommand(saleId, request);

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.SaleId },
                result
            );
        }
        
        [HttpDelete("{saleId:guid}/items/{itemId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveItem(
            [FromRoute] Guid saleId,
            [FromRoute] Guid itemId,
            CancellationToken ct)
        {
            var command = new RemoveSaleItem.RemoveSaleItemCommand(saleId, itemId);

            await _mediator.Send(command, ct);

            return NoContent();
        }
        
        [HttpPost("{saleId:guid}/payments")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddPayment(
            [FromRoute] Guid saleId,
            [FromBody] AddPaymentRequest request,
            CancellationToken ct)
        {
            var command = new AddPayment.AddPaymentCommand(saleId, request);

            var result = await _mediator.Send(command, ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.SaleId },
                result
            );
        }
        
        [HttpPatch("{id:guid}/finalize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Finalize([FromRoute] Guid id, CancellationToken ct)
        {
            var command = new FinalizeSale.FinalizeSaleCommand(id);

            var result = await _mediator.Send(command, ct);

            return Ok(result);
        }
        
        [HttpPatch("{id:guid}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel([FromRoute] Guid id, CancellationToken ct)
        {
            var command = new CancelSale.CancelSaleCommand(id);

            await _mediator.Send(command, ct);

            return NoContent();
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(
            CancellationToken ct,
            [FromQuery] Guid? cashRegisterId,
            [FromQuery] Guid? sellerId,
            [FromQuery] string? saleNumber,
            [FromQuery] SaleStatus? status,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllSale.GetAllSaleQuery(
                cashRegisterId, sellerId, saleNumber, status, startDate, endDate, page, pageSize);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }
    }
}