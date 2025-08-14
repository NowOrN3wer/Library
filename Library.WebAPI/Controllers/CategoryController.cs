using Library.Application.Features.Categories.Commands.Add;
using Library.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers;

[Authorize]
public sealed class CategoryController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}