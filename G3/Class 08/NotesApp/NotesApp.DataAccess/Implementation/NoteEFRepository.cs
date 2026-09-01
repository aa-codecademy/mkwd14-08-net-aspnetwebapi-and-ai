using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementation
{
    public class NoteEFRepository : INoteRepository
    {
        private readonly NoteDbContext _dbContext;

        public NoteEFRepository(NoteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Note entity)
        {
            _dbContext.Notes.Add(entity); //here the changes have not yet been saved to the db
            _dbContext.SaveChanges(); //we need to call SaveChanges in order to actually save them in the db
        }

        public void Delete(int id)
        {
            //check if you can find it by id
            var note = GetById(id);

            if (note != null)
            {
                _dbContext.Notes.Remove(note); //here we call remove, but this is not yet affecting the db
                _dbContext.SaveChanges(); //we need to call save changes in order to save the changes in the db
            }
        }

        public List<Note> GetAll()
        {
            return _dbContext.Notes
                .Include(x => x.Tags) //in the background this means do a join in db to get the tags
                .Include( x => x.User)
                   //.ThenInclude(x => x.Role) //this means from User do a join with Role (not from note, but from user)
                .ToList();
        }

        public Note GetById(int id)
        {
            return _dbContext.Notes
                 .Include(x => x.Tags)
                 .Include(x => x.User)
                 .FirstOrDefault(x => x.Id == id);
        }
         
        public void Update(Note entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("Note cannot be null");
            }

            _dbContext.Notes.Update(entity); //the db has not been changed yet
            _dbContext.SaveChanges(); //we need to call save changes in order to save the changes in the db
        }
    }
}
