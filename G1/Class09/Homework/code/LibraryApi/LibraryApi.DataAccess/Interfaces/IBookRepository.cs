using LibraryApi.Domain.Models;

namespace LibraryApi.DataAccess.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    Task<List<Book>> GetByAuthorIdAsync(int authorId);
}
