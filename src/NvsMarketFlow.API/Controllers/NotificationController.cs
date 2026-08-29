using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.UseCases.Notification.Commands;
using NvsMarketFlow.Application.UseCases.Notification.Queries;

namespace NvsMarketFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(
            CancellationToken ct,
            [FromQuery] Guid userId,
            [FromQuery] bool? read,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllNotification.GetAllNotificationQuery(read, page, pageSize);

            var result = await _mediator.Send(query, ct);

            return Ok(result);
        }

        [HttpPatch("{id:guid}/read")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsRead([FromRoute] Guid id, CancellationToken ct)
        {
            var command = new MarkNotificationAsRead.MarkNotificationAsReadCommand(id);

            await _mediator.Send(command, ct);

            return NoContent();
        }
    }
}