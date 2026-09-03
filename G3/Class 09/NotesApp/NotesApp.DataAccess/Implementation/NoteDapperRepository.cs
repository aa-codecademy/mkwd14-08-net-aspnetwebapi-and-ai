using System.Reflection.Metadata.Ecma335;
using Dapper;
using Microsoft.Data.SqlClient;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementation
{
    public class NoteDapperRepository : INoteRepository
    {
        private readonly string _connectionString;

        public NoteDapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Note entity)
        {
            using var sqlConnection = new SqlConnection(_connectionString);

            //we need to write the sql query on our own
            //ALWAYS use sql params in order to avoid sql injection
            string query = @"
            INSERT INTO dbo.Notes(Text, Priority, UserId)
            VALUES (@Text, @Priority, @UserId)";

            //we use Execute when the db returns rows affected
            sqlConnection.Execute(query, new //this way we send the values for the params
            {
                entity.Text,
                entity.Priority,
                entity.UserId
            });
        }

        public void Delete(int id)
        {
            using var sqlConenction = new SqlConnection(_connectionString);

            string deleteTagsQuery = @"
                DELETE FROM dbo.NoteTags
                WHERE NotesId = @Id";

            //delete returns rows affected, so here we use Execute
            sqlConenction.Execute(deleteTagsQuery, new { Id = id });

            string deleteNote = @"
                DELETE FROM dbo.Notes
                WHERE Id = @Id";

            sqlConenction.Execute(deleteNote, new { Id = id });
        }

        public List<Note> GetAll()
        {
            using var connection = new SqlConnection(_connectionString);

            //we need to write the sql query ourselves
            string query = @"
            SELECT *
            FROM dbo.Notes";

            //we use Query when the db returns rows of data that need to be mapped into an object
            //Dapper does the mapping for us
            return connection.Query<Note>(query).ToList();
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
