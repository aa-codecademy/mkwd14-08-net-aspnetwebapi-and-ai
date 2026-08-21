using NotesApp.DataAccess.Data;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementation
{
    public class UserRepository : IUserRepository
    {
        public void Add(User entity)
        {
            //we still don't have a connection to a real db, so we need to manually handle the value for the id
            entity.Id = StaticDb.Users.LastOrDefault() != null ? StaticDb.Users.LastOrDefault().Id + 1: 1;
            StaticDb.Users.Add(entity);
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            if (user != null)
            {

                StaticDb.Users.Remove(user);
            }
        }

        public List<User> GetALL()
        {
            return StaticDb.Users;
        }

        public User GetById(int id)
        {
            return StaticDb.Users.FirstOrDefault(x => x.Id == id);
        }

        public void Update(User entity)
        {
            if (entity == null)
            {

                throw new Exception("Entity cannot be null!");
            }

            //because we have a List (collection) in db - we need to find the item that we will replace with its updated object
            int index = StaticDb.Users.FindIndex(user => user.Id == entity.Id);

            //if we find an item with that id
            if (index != -1)
            {
                //in its place we put the new updated version of that object
                StaticDb.Users[index] = entity;
            }
        }
    }
}
