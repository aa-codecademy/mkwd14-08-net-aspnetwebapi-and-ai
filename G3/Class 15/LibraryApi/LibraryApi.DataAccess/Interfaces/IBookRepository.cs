using LibraryApi.Domain.Models;

namespace LibraryApi.DataAccess.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        List<Book> GetByAuthorId(int authorId);
    }
}
