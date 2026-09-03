using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementation
{
    public class UserEFRepository : IUserRepository
    {
        private readonly NoteDbContext _dbContext;

        public UserEFRepository(NoteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(User entity)
        {
            _dbContext.Users.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
        }

        public List<User> GetAll()
        {
            return _dbContext.Users
                .Include(x => x.Notes)
                .ToList();
        }

        public User GetById(int id)
        {
            return _dbContext.Users
                .Include(x => x.Notes)
                .FirstOrDefault(x => x.Id == id);
        }

        public void Update(User entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("User cannot be null");
            }

            _dbContext.Users.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
