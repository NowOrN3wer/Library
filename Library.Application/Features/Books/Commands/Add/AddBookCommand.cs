using MediatR;
using TS.Result;

namespace Library.Application.Features.Books.Commands.Add;

public  sealed record AddBookCommand : IRequest<Result<bool>>
{
    
}