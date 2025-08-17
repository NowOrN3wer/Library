using Library.Application.Features.Auth.Login;
using Library.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers;

[AllowAnonymous]
public sealed class AuthController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Login(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}