using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure.Context;
using Library.Infrastructure.Persistence.Repositories;

namespace Library.Infrastructure.Repositories;

internal sealed class WriterRepository(ApplicationDbContext context)
    : ExtendedRepository<Writer>(context), IWriterRepository;