using System.Net.Http.Headers;
using NotesAppClientApi.Models;

namespace NotesAppClientApi.Services;

/// <summary>
/// Everything this API knows about the Notes API lives here - the URLs, the JSON
/// and the token. It never creates an HttpClient: one arrives through the
/// constructor, and WHERE it comes from is what the three ways are about.
/// </summary>
public class NotesService
{

    public NotesService()
    {
    }

    /// <summary>Logs in and keeps the token for every later call.</summary>
    public async Task LoginAsync(string username, string password)
    {
        LoginRequest credentials = new LoginRequest
        {
            Username = username,
            Password = password
        };

      
    }

    public async Task<List<NoteDto>> GetNotesAsync()
    {

        return new List<NoteDto>();
    }
}
