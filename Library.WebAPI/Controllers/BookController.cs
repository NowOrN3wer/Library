using Library.Application.Features.Books.Commands.Add;
using Library.Application.Features.Books.Queries.GetById;
using Library.Application.Features.Books.Queries.GetPage;
using Library.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers;

[AllowAnonymous]
public sealed class BookController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddBookCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> GetPage([FromBody] GetPageBookQuery request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetByIdBookQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}