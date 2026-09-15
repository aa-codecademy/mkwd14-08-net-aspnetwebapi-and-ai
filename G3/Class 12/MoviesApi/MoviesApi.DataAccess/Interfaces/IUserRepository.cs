using MoviesApi.Domain.Models;

namespace MoviesApi.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByUsername(string username);
    }
}
