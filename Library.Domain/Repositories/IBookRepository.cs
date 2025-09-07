using Library.Domain.Common.Interfaces;
using Library.Domain.Entities;

namespace Library.Domain.Repositories;

public interface IBookRepository : IExtendedRepository<Book>
{
}