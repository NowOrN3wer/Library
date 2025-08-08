using Library.Application.Dto.WriterDtos;
using MediatR;
using TS.Result;

namespace Library.Application.Features.Writers.Queries.GetById;

public sealed record GetByIdWriterQuery(Guid Id) : IRequest<Result<WriterDto>>;