using Library.Application.Features.ApiLogs.GetPage;
using Library.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers;

public sealed class ApiLogController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> GetPage(GetPageApiLogCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}