using NotesApp.DataAccess.Data;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementation
{
    public class NoteRepository : INoteRepository
    {
        public void Add(Note entity)
        {
            //we still don't have a connection to a real db, so we need to manually handle the value for the id
            entity.Id = StaticDb.Notes.LastOrDefault() != null ? StaticDb.Notes.LastOrDefault().Id + 1 : 1;
            StaticDb.Notes.Add(entity);
        }

        public void Delete(int id)
        {
            var note = GetById(id);
            if (note != null)
            {
                StaticDb.Notes.Remove(note);
            }
        }

        public List<Note> GetAll()
        {
            return StaticDb.Notes;
        }

        public Note GetById(int id)
        {
            return StaticDb.Notes.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Note entity)
        {
            if (entity == null)
            {

                throw new Exception("Entity cannot be null!");
            }

            //because we have a List (collection) in db - we need to find the item that we will replace with its updated object
            int index = StaticDb.Notes.FindIndex(note => note.Id == entity.Id);

            //if we find an item with that id
            if (index != -1)
            {
                //in its place we put the new updated version of that object
                StaticDb.Notes[index] = entity;
            }
        }
    }
}
