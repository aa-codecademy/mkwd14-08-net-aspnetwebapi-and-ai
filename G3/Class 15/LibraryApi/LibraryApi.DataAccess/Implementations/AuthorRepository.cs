using LibraryApi.DataAccess.Data;
using LibraryApi.DataAccess.Interfaces;
using LibraryApi.Domain.Models;

namespace LibraryApi.DataAccess.Implementations
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public List<Author> GetAll()
        {
            return _context.Authors.ToList();
        }

        public Author? GetById(int id)
        {
            return _context.Authors.FirstOrDefault(author => author.Id == id);
        }

        public void Add(Author entity)
        {
            _context.Authors.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Author entity)
        {
            _context.SaveChanges();
        }

        public void Delete(Author entity)
        {
            _context.Authors.Remove(entity);
            _context.SaveChanges();
        }
    }
}
