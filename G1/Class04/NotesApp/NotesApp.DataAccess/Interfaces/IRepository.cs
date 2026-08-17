using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    List<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
