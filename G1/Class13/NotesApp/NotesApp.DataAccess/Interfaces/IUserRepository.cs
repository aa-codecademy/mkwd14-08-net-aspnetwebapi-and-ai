using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<bool> CheckUsernameExistsAsync(string username);
    Task<User?> GetByUsernameAsync(string username);
}
