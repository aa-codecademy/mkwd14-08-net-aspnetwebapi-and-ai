using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using System.Data;

namespace NotesApp.DataAccess.Implementations.AdoNet;

// ADO.NET is a low-level data access technology that allows you to interact with databases using SQL commands and ADO.NET objects. It provides a set of classes in the System.Data namespace that enable you to connect to a database, execute commands, and retrieve results.

// SqlConnection => used to establish a connection to a SQL Server database
// SqlCommand => execute SQL queries, stored procedures, and other database commands
// SqlDataReader => read data from a database
public class NoteRepositoryAdoNet : INoteRepository
{
    private readonly string _connectionString;

    // The IConfiguration interface is part of the Microsoft.Extensions.Configuration namespace and is used to access configuration settings in .NET applications. It provides a way to retrieve configuration values from various sources, such as appsettings.json, environment variables, command-line arguments, and more.
    public NoteRepositoryAdoNet(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("NotesAppDb") ?? throw new InvalidOperationException("Connection string 'NotesAppDb' not found.");
        // TODO: Improve this using IOptions pattern
    }

    private const string SelectNotesSql = @"
        SELECT  n.Id          AS NoteId,
                n.Text        AS NoteText,
                n.Priority    AS NotePriority,
                n.UserId      AS NoteUserId,
                n.CreatedDate AS NoteCreatedDate,
                n.UpdatedDate AS NoteUpdatedDate,
                u.Id          AS UserId,
                u.FirstName   AS UserFirstName,
                u.LastName    AS UserLastName,
                u.Username    AS UserUsername,
                t.Id          AS TagId,
                t.Name        AS TagName,
                t.Color       AS TagColor
        FROM       dbo.Note    n
        LEFT JOIN  dbo.[User]  u  ON u.Id = n.UserId
        LEFT JOIN  dbo.NoteTag nt ON nt.NoteId = n.Id
        LEFT JOIN  dbo.Tag     t  ON t.Id = nt.TagId";

    public async Task<List<Note>> GetAllAsync()
    {
        // 1) Create a connection to the database
        using SqlConnection connection = new SqlConnection(connectionString: _connectionString);
        await connection.OpenAsync();

        // 2) Create sql query 
        string sqlQuery = SelectNotesSql + " ORDER BY n.Id";

        // 3) Create a command to execute the query
        using SqlCommand command = new SqlCommand(sqlQuery, connection);

        // 4) Execute the command and read the results  
        using SqlDataReader reader = await command.ExecuteReaderAsync();

        // 5) Process the results and map them to Note objects
        var notes = await ReadNotesAsync(reader);
        return notes;
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using SqlCommand command = new SqlCommand(SelectNotesSql + " WHERE n.Id = @Id;", connection);

        command.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = await command.ExecuteReaderAsync();

        List<Note> notes = await ReadNotesAsync(reader);

        return notes.FirstOrDefault();
    }

    public async Task<List<Note>> GetByIdsAsync(List<int> ids)
    {
        List<Note> notes = new List<Note>();

        foreach (int id in ids)
        {
            Note? note = await GetByIdAsync(id);

            if (note is not null)
            {
                notes.Add(note);
            }
        }

        return notes;
    }

    public async Task AddAsync(Note entity)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        using SqlTransaction transaction = connection.BeginTransaction();

        const string insertNoteSql = @"
            INSERT INTO dbo.Note (Text, Priority, UserId, CreatedDate, UpdatedDate)
            OUTPUT INSERTED.Id
            VALUES (@Text, @Priority, @UserId, @CreatedDate, @UpdatedDate);";

        using (SqlCommand command = new SqlCommand(insertNoteSql, connection, transaction))
        {
            AddNoteParameters(command, entity);

            object? newId = await command.ExecuteScalarAsync();
            entity.Id = Convert.ToInt32(newId);
        }

        await InsertTagsAsync(connection, transaction, entity);

        await transaction.CommitAsync();
    }

    public async Task UpdateAsync(Note entity)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using SqlTransaction transaction = connection.BeginTransaction();

        entity.UpdatedDate = DateTime.UtcNow;

        const string updateNoteSql = @"
            UPDATE dbo.Note
            SET    Text        = @Text,
                   Priority    = @Priority,
                   UpdatedDate = @UpdatedDate
            WHERE  Id = @Id;";

        using (SqlCommand command = new SqlCommand(updateNoteSql, connection, transaction))
        {
            command.Parameters.AddWithValue("@Text", entity.Text);
            command.Parameters.AddWithValue("@Priority", entity.Priority.ToString());
            command.Parameters.Add("@UpdatedDate", SqlDbType.DateTime2).Value = entity.UpdatedDate;
            command.Parameters.AddWithValue("@Id", entity.Id);

            await command.ExecuteNonQueryAsync();
        }

        using (SqlCommand command = new SqlCommand(
            "DELETE FROM dbo.NoteTag WHERE NoteId = @NoteId;", connection, transaction))
        {
            command.Parameters.AddWithValue("@NoteId", entity.Id);
            await command.ExecuteNonQueryAsync();
        }

        await InsertTagsAsync(connection, transaction, entity);

        await transaction.CommitAsync();
    }

    public async Task DeleteAsync(Note entity)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string deleteSql = "DELETE FROM dbo.Note WHERE Id = @Id;";

        using SqlCommand command = new SqlCommand(deleteSql, connection);
        command.Parameters.AddWithValue("@Id", entity.Id);

        await command.ExecuteNonQueryAsync();
    }

    #region Helper methods
    private static async Task<List<Note>> ReadNotesAsync(SqlDataReader reader)
    {
        Dictionary<int, Note> notesById = new Dictionary<int, Note>();

        while (await reader.ReadAsync())
        {
            int noteId = (int)reader["NoteId"];

            if (!notesById.TryGetValue(noteId, out Note? note))
            {
                note = new Note
                {
                    Id = noteId,
                    Text = (string)reader["NoteText"],

                    Priority = Enum.Parse<Priority>((string)reader["NotePriority"]),

                    UserId = reader["NoteUserId"] as int?,

                    CreatedDate = (DateTime)reader["NoteCreatedDate"],
                    UpdatedDate = (DateTime)reader["NoteUpdatedDate"]
                };

                if (reader["UserId"] is int userId)
                {
                    note.User = new User
                    {
                        Id = userId,
                        FirstName = (string)reader["UserFirstName"],
                        LastName = (string)reader["UserLastName"],
                        Username = (string)reader["UserUsername"]
                    };
                }

                notesById.Add(noteId, note);
            }

            if (reader["TagId"] is int tagId)
            {
                note.Tags.Add(new Tag
                {
                    Id = tagId,
                    Name = (string)reader["TagName"],
                    Color = (string)reader["TagColor"]
                });
            }
        }

        return notesById.Values.ToList();
    }

    private static void AddNoteParameters(SqlCommand command, Note entity)
    {
        command.Parameters.AddWithValue("@Text", entity.Text);

        command.Parameters.AddWithValue("@Priority", entity.Priority.ToString());

        command.Parameters.AddWithValue("@UserId", (object?)entity.UserId ?? DBNull.Value);

        command.Parameters.Add("@CreatedDate", SqlDbType.DateTime2).Value = entity.CreatedDate;
        command.Parameters.Add("@UpdatedDate", SqlDbType.DateTime2).Value = entity.UpdatedDate;
    }

    private static async Task InsertTagsAsync(
       SqlConnection connection, SqlTransaction transaction, Note entity)
    {
        const string insertTagSql =
            "INSERT INTO dbo.NoteTag (NoteId, TagId) VALUES (@NoteId, @TagId);";

        // One round trip per tag. Fine for five, wrong for five thousand.
        foreach (Tag tag in entity.Tags)
        {
            using SqlCommand command = new SqlCommand(insertTagSql, connection, transaction);
            command.Parameters.AddWithValue("@NoteId", entity.Id);
            command.Parameters.AddWithValue("@TagId", tag.Id);

            await command.ExecuteNonQueryAsync();
        }
    }
    #endregion
}
