using Microsoft.Data.SqlClient;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementation
{
    public class NoteAdoRepository : INoteRepository
    {
        private readonly string _connectionString;

        public NoteAdoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Note entity)
        {
            //create the sql connection - we need a connection string
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);

            //open the connection
            sqlConnection.Open();

            //create the command - we need to dispose this resource at the end, so using is our best practise
            using SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;

            //this is the query that we would send to the db
            //we should ALWAYS use parameters to avoid sql injection
            command.CommandText = @"
            INSERT INTO dbo.Notes (Text, Priority, UserId)
            VALUES(@text, @priority, @userId)";

            command.Parameters.AddWithValue("@text", entity.Text);
            command.Parameters.AddWithValue("@priority", entity.Priority);
            command.Parameters.AddWithValue("@userId", entity.UserId);

            command.ExecuteNonQuery(); //here we don't need a reader, beacuse the db returns only rows affected. When we only expect rows affected we use ExecuteNonQuery
        }

        public void Delete(int id)
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();

            //we need to check in NoteTags table, if there are any records with this noteId we need to delete them first because of our FK constraint
            using SqlCommand tagCommand = new SqlCommand();
            tagCommand.Connection = sqlConnection;

            //delete the records from NoteTags with this NoteId
            tagCommand.CommandText = @"
            DELETE FROM dbo.NoteTags
            WHERE NoteId = @noteId";

            tagCommand.Parameters.AddWithValue("@noteId", id);

            tagCommand.ExecuteNonQuery(); //delete return only rows affected so here we use ExecuteNonQuery

            //after we deleted the relationships that this note had, now we can delete the actual note
            using SqlCommand deleteCommand = new SqlCommand();
            deleteCommand.Connection = sqlConnection;

            deleteCommand.CommandText = @"
                DELETE FROM dbo.Notes
                WHERE Id = @id";

            deleteCommand.Parameters.AddWithValue("@id", id);
            deleteCommand.ExecuteNonQuery(); //delete return only rows affected so here we use ExecuteNonQuery
        }

        public List<Note> GetAll()
        {
            using SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();

            using SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;

            command.CommandText = @"
            SELECT 
                N.Id AS NoteId,
                N.Text,
                N.Priority,
                N.UserId,
                U.FirstName,
                U.LastName,
                U.Username,
                U.Password,
                T.Id AS TagId,
                T.Name,
                T.Color

            FROM dbo.Notes N

            INNER JOIN dbo.Users U
                ON U.Id = N.UserId
            
            LEFT JOIN dbo.NoteTags NT
                ON N.Id = NT.NotesId

            LEFT JOIN dbo.Tags T
                ON T.Id = NT.TagsId";


            //because SELECT returns actual data rows - we need a reader
            using SqlDataReader reader = command.ExecuteReader();

            List<Note> notes = new List<Note>();    
            while (reader.Read()) {

                int noteId = (int)reader["NoteId"];

                //check if we already read this one and it already exists in our list
                Note note = notes.FirstOrDefault(x => x.Id == noteId);  

                if(note == null)
                {
                    note = new Note
                    {
                        Id = noteId,
                        Text = (string)reader["Text"],
                        Priority = (PriorityEnum)reader["Priority"],
                        UserId = (int)reader["UserId"],
                        User = new User
                        {
                            Id = (int)reader["UserId"],
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Username = (string)reader["Username"],
                            Password = (string)reader["Password"]
                        }
                    };

                    notes.Add(note); //here we only read the note - we have not yet read the tags
                }

                //beacuse we have left join, we might not have tags to read - so we check if our note has tags
                if (!reader.IsDBNull(reader.GetOrdinal("TagId")))
                {
                    Tag tag = new Tag
                    {
                        Id = (int)reader["TagId"],
                        Name = (string)reader["Name"],
                        Color = (string)reader["Color"]
                    };
                    note.Tags.Add(tag); //now we have read the tags as well
                }
            }

            return notes;
        }

        public Note GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Note entity)
        {
            throw new NotImplementedException();
        }
    }
}
