using NotesApp.DataAccess.Data;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementations;

public class NoteRepository : INoteRepository
{
    public List<Note> GetAll()
    {
        return StaticDb.Notes;
    }

    public Note? GetById(int id)
    {
        return StaticDb.Notes.FirstOrDefault(note => note.Id == id);
    }

    public void Add(Note entity)
    {
        entity.Id = StaticDb.NextNoteId();
        StaticDb.Notes.Add(entity);
    }

    public void Update(Note entity)
    {
        int index = StaticDb.Notes.FindIndex(note => note.Id == entity.Id);
        if (index >= 0)
        {
            StaticDb.Notes[index] = entity;
        }
    }

    public void Delete(Note entity)
    {
        StaticDb.Notes.Remove(entity);
    }

    public List<Note> GetByIds(List<int> ids)
    {
        throw new NotImplementedException();
    }
}
