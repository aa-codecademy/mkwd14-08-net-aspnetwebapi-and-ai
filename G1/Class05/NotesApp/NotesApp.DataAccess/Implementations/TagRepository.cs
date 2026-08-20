using NotesApp.DataAccess.Data;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementations;

public class TagRepository : ITagRepository
{
    public void Add(Tag entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Tag entity)
    {
        throw new NotImplementedException();
    }

    public List<Tag> GetAll()
    {
        throw new NotImplementedException();
    }

    public Tag? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public List<Tag> GetByIds(List<int> ids)
    {
        return StaticDb.Tags.Where(tag => ids.Contains(tag.Id)).ToList();
    }

    public void Update(Tag entity)
    {
        throw new NotImplementedException();
    }
}
