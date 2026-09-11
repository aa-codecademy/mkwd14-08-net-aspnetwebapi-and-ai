using MoviesApi.Domain.Models;

namespace MoviesApi.DataAccess.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        //CRUD
        List<T> GetAll();
        T GetById(int id);
        void Add(T entity); 
        void Update(T entity);
        void Delete(T entity);
    }
}
