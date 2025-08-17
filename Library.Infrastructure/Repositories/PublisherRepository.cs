using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure.Context;
using Library.Infrastructure.Persistence.Repositories;

namespace Library.Infrastructure.Repositories;

internal sealed class PublisherRepository(ApplicationDbContext context): ExtendedRepository<Publisher>(context), IPublisherRepository;