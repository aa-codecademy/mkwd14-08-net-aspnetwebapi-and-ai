using Microsoft.EntityFrameworkCore;
using MoviesApi.DataAccess.Interfaces;
using MoviesApi.Domain.Models;

namespace MoviesApi.DataAccess.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly MovieDbContext _context;

        public UserRepository(MovieDbContext context)
        {
            _context = context;
        }

        public void Add(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(User entity)
        {
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _context.Users.Include(x => x.Movies).ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Include(x => x.Movies).FirstOrDefault(x => x.Id == id);
        }

        public void Update(User entity)
        {
            _context.Users.Update(entity);
            _context.SaveChanges();
        }
    }
}
