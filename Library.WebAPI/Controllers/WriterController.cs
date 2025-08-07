using Library.Application.Features.Writers.Add;
using Library.Application.Features.Writers.Delete;
using Library.Application.Features.Writers.GetPage;
using Library.Application.Features.Writers.Update;
using Library.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers;

[AllowAnonymous]
public class WriterController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddWriterCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }    
    
    [HttpPost]
    public async Task<IActionResult> GetPage([FromBody] GetPageWriterCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }    
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateWriterCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }    
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteWriterCommand(id), cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}