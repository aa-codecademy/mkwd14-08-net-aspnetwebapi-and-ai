using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementation
{
    public class TagEFRepository : ITagRepository
    {
        private readonly NoteDbContext _dbContext;

        public TagEFRepository(NoteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Tag entity)
        {
            _dbContext.Tags.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var tag = GetById(id);
            if (tag != null)
            {
                _dbContext.Tags.Remove(tag);
                _dbContext.SaveChanges();
            }
        }

        public List<Tag> GetAll()
        {
           return _dbContext.Tags
                .Include(x => x.Notes) //if we want to eager load all the notes for tag, we include (join) the notes here
                .ToList();
        }

        public Tag GetById(int id)
        {
            return _dbContext.Tags //here we did not load the notes, so tag.Notes.Select would be null.Select and would cause an error
                .FirstOrDefault(x => x.Id == id);
        }

        public void Update(Tag entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Tag cannot be null");
            }

            _dbContext.Tags.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
