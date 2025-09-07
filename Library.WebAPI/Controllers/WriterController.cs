using Library.Application.Features.Writers.Commands.Add;
using Library.Application.Features.Writers.Commands.Delete;
using Library.Application.Features.Writers.Commands.Restore;
using Library.Application.Features.Writers.Commands.Update;
using Library.Application.Features.Writers.Queries.GetById;
using Library.Application.Features.Writers.Queries.GetLookup;
using Library.Application.Features.Writers.Queries.GetPage;
using Library.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers;

[Authorize]
public sealed class WriterController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddWriterCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> GetPage([FromBody] GetPageWriterQuery request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateWriterCommand request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new DeleteWriterCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetByIdWriterQuery(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Restore([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new RestoreWriterCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Lookup([FromBody] GetWriterLookupQuery request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}