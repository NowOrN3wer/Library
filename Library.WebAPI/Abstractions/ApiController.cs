using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Abstractions;

[Route("api/[controller]/[action]")]
[ApiController]
public abstract class ApiController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator _mediator = mediator;
}