using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NvsMarketFlow.Application.Requests.Auth;
using NvsMarketFlow.Application.UseCases.Auth.Commands;

namespace NvsMarketFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var command = new Login.LoginCommand(request);
            var result = await _mediator.Send(command, ct);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
        {
            var command = new RefreshAccessToken.RefreshAccessTokenCommand(request);
            var result = await _mediator.Send(command, ct);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken ct)
        {
            var command = new Logout.LogoutCommand(request);
            await _mediator.Send(command, ct);
            return NoContent();
        }
    }
}