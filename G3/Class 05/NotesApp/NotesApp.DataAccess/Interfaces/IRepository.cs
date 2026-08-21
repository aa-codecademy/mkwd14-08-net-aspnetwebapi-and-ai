using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        //These CRUD methods are used for all entities
        //that's why we keep them in this "parent" interface
        List<T> GetALL();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
