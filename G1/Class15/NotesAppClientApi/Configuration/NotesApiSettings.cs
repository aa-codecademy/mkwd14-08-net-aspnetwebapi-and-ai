namespace NotesAppClientApi.Configuration;

/// <summary>
/// The "NotesApi" section of appsettings.json, as a class.
/// Program.cs binds it, everyone else reads it through IOptions - same pattern
/// as JwtSettings in the Notes API.
/// </summary>
public class NotesApiSettings
{
    /// <summary>Where the Notes API lives. Must end with a slash.</summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>The account this API logs in with. Demo only - see appsettings.json.</summary>
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
