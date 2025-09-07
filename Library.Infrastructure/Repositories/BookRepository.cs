using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure.Context;
using Library.Infrastructure.Persistence.Repositories;

namespace Library.Infrastructure.Repositories;

internal sealed class BookRepository(ApplicationDbContext context) : ExtendedRepository<Book>(context), IBookRepository;