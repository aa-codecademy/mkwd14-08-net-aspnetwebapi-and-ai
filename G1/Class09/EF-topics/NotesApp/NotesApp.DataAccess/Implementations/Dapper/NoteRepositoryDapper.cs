using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;

namespace NotesApp.DataAccess.Implementations.Dapper;

// Dapper is a lightweight ORM (Object-Relational Mapper) for .NET that provides a simple and efficient way to interact with databases.
// It is designed to be fast and easy to use, allowing developers to execute SQL queries and map the results to C# objects with minimal overhead.
// Dapper is often used in scenarios where performance is critical, and it works well with existing ADO.NET code.
public class NoteRepositoryDapper : INoteRepository
{
    private readonly string _connectionString;

    public NoteRepositoryDapper(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("NotesAppDb") ?? throw new InvalidOperationException("Connection string 'NotesAppDb' not found.");
    }

    private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<List<Note>> GetAllAsync()
    {
        using SqlConnection connection = CreateConnection();
        string query = "SELECT * FROM dbo.Note";
        IEnumerable<Note> notes = await connection.QueryAsync<Note>(query);
        return notes.ToList();

        // With User and Tags: swap for this and uncomment the block at the bottom.
        //return await QueryNotesWithRelationsAsync(
        //    connection, 
        //    SelectNotesWithRelationsSql + " ORDER BY n.Id;", 
        //    parameters: null
        //);
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        using SqlConnection connection = CreateConnection();

        // The anonymous object becomes @Id - parameterised, so no SQL injection.
        return await connection.QueryFirstOrDefaultAsync<Note>(
            "SELECT * FROM dbo.Note WHERE Id = @Id", new { Id = id });

        // With User and Tags: swap for this and uncomment the block at the bottom.
        //List<Note> notes = await QueryNotesWithRelationsAsync(
        //    connection, SelectNotesWithRelationsSql + " WHERE n.Id = @Id;", new { Id = id });
        //return notes.FirstOrDefault();
    }

    public async Task<List<Note>> GetByIdsAsync(List<int> ids)
    {
        using SqlConnection connection = CreateConnection();
        string query = "SELECT * FROM dbo.Note WHERE Id IN @Ids";
        return (await connection.QueryAsync<Note>(query, new { Ids = ids })).ToList();
    }

    public async Task AddAsync(Note entity)
    {
        using SqlConnection connection = CreateConnection();
        await connection.OpenAsync();

        // Note + tag rows are one change, so one transaction.
        // If everything succeeds, commit. If anything fails, rollback.
        using SqlTransaction transaction = connection.BeginTransaction();

        // SQL Server stamps the dates, OUTPUT hands them back with the new Id.
        const string insertQuery = @"
            INSERT INTO dbo.Note (Text, Priority, UserId, CreatedDate, UpdatedDate)
            OUTPUT INSERTED.Id, INSERTED.CreatedDate, INSERTED.UpdatedDate
            VALUES (@Text, @Priority, @UserId, GETUTCDATE(), GETUTCDATE());
        ";

        // Priority.ToString() because the column is nvarchar - what HasConversion<string>() did.
        Note inserted = await connection.QuerySingleAsync<Note>(
            sql: insertQuery,
            param: new { entity.Text, Priority = entity.Priority.ToString(), entity.UserId },
            transaction
        );

        entity.Id = inserted.Id;
        entity.CreatedDate = inserted.CreatedDate;
        entity.UpdatedDate = inserted.UpdatedDate;

        await SaveTagsAsync(connection, transaction, entity);

        await transaction.CommitAsync();
    }

    private static async Task SaveTagsAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        Note entity
    )
    {
        await connection.ExecuteAsync("DELETE FROM dbo.NoteTag WHERE NoteId = @NoteId", new { NoteId = entity.Id }, transaction);

        if (entity.Tags.Count == 0)
        {
            return;
        }

        await connection.ExecuteAsync(
            "INSERT INTO dbo.NoteTag (NoteId, TagId) VALUES (@NoteId, @TagId);",
            param: entity.Tags.Select(tag => new { TagId = tag.Id, NoteId = entity.Id }),
            transaction
        );
    }

    public async Task UpdateAsync(Note entity)
    {
        using SqlConnection connection = CreateConnection();
        string query = @"
            UPDATE dbo.Note
            SET Text = @Text,
                Priority = @Priority,
                UserId = @UserId,
                UpdatedDate = GETUTCDATE()
            WHERE Id = @Id;
        ";

        await connection.ExecuteAsync(
            query,
            new { entity.Text, Priority = entity.Priority.ToString(), entity.UserId, Id = entity.Id }
        );
    }

    public async Task DeleteAsync(Note entity)
    {
        using SqlConnection connection = CreateConnection();
        await connection.ExecuteAsync("DELETE FROM dbo.Note WHERE Id = @Id", new { Id = entity.Id });
    }

    // OPTIONAL: the same reads, with User and Tags loaded.
    // Uncomment this block plus the two lines in GetAllAsync / GetByIdAsync.

    // Dapper maps by name, so the column names must be the property names.
    private const string SelectNotesWithRelationsSql = @"
        SELECT  n.Id, n.Text, n.Priority, n.UserId, n.CreatedDate, n.UpdatedDate,
                u.Id, u.FirstName, u.LastName, u.Username, u.CreatedDate, u.UpdatedDate,
                t.Id, t.Name, t.Color, t.CreatedDate, t.UpdatedDate
        FROM       dbo.Note    n
        LEFT JOIN  dbo.[User]  u  ON u.Id = n.UserId
        LEFT JOIN  dbo.NoteTag nt ON nt.NoteId = n.Id
        LEFT JOIN  dbo.Tag     t  ON t.Id = nt.TagId";

    // splitOn "Id,Id" says where to cut each row into Note / User / Tag, so the
    // SELECT column order is load-bearing. The join repeats a note once per tag -
    // that is what the dictionary is for.
    private static async Task<List<Note>> QueryNotesWithRelationsAsync(
        SqlConnection connection, string sql, object? parameters)
    {
        Dictionary<int, Note> notesById = new Dictionary<int, Note>();

        await connection.QueryAsync<Note, User, Tag, Note>(
            sql,
            (note, user, tag) =>
            {
                if (notesById.TryGetValue(note.Id, out Note? alreadyBuilt))
                {
                    note = alreadyBuilt;
                }
                else
                {
                    note.User = user;   // null on a LEFT JOIN miss
                    notesById.Add(note.Id, note);
                }

                if (tag is not null)
                {
                    note.Tags.Add(tag);
                }

                return note;
            },
            parameters,
            splitOn: "Id,Id");

        return notesById.Values.ToList();
    }
}