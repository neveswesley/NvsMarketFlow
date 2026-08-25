using MediatR;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.Requests.User;
using NvsMarketFlow.Application.UseCases.User.Commands;
using NvsMarketFlow.Application.UseCases.User.Query;

namespace NvsMarketFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateUser.CreateUserCommand(request);
            
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
            var query = new GetUserById.GetUserByIdQuery(id, cancellationToken);
            
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
            var query = new GetAllUsers.GetAllUsersQuery(name, page, pageSize);
            
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteUser.DeleteUserCommand(id);
            
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPatch("{id:guid}/email")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateEmail([FromRoute] Guid id, [FromBody] UpdateEmailRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateEmail.UpdateEmailCommand(id, request);
            
            await _mediator.Send(command, cancellationToken);
            
            return NoContent();
        }
        
        [HttpPatch("{id:guid}/name")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateName([FromRoute] Guid id, [FromBody] UpdateNameRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateName.UpdateNameCommand(id, request);
            
            await _mediator.Send(command, cancellationToken);
            
            return NoContent();
        }
        
        [HttpPatch("{id:guid}/password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ChangePassword([FromRoute] Guid id, [FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var command = new ChangePassword.ChangePasswordCommand(id, request);
            
            await _mediator.Send(command, cancellationToken);
            
            return NoContent();
        }
        
        
        
    }
}
