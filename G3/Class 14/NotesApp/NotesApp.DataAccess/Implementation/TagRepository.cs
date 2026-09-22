using NotesApp.DataAccess.Data;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementation
{
    public class TagRepository : ITagRepository
    {
        public void Add(Tag entity)
        {
            //we still don't have a connection to a real db, so we need to manually handle the value for the id
            entity.Id = StaticDb.Tags.LastOrDefault() != null ? StaticDb.Tags.LastOrDefault().Id + 1 : 1;
            StaticDb.Tags.Add(entity);
        }

        public void Delete(int id)
        {
            var tag = GetById(id);
            if (tag != null)
            {

                StaticDb.Tags.Remove(tag);
            }
        }

        public List<Tag> GetAll()
        {
            return StaticDb.Tags;
        }

        public Tag GetById(int id)
        {
            return StaticDb.Tags.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Tag entity)
        {
            if (entity == null)
            {

                throw new Exception("Entity cannot be null!");
            }

            //because we have a List (collection) in db - we need to find the item that we will replace with its updated object
            int index = StaticDb.Tags.FindIndex(tag => tag.Id == entity.Id);

            //if we find an item with that id
            if (index != -1)
            {
                //in its place we put the new updated version of that object
                StaticDb.Tags[index] = entity;
            }
        }
    }
}
