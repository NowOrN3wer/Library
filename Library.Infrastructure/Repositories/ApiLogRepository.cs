using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure.Common;
using Library.Infrastructure.Context;

namespace Library.Infrastructure.Repositories;

internal sealed class ApiLogRepository(ApplicationDbContext context)
    : PagedRepository<ApiLog>(context), IApiLogRepository;