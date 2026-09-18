namespace NotesAppClientApi.Models;

// What we SEND to the Notes API to log in. Same shape as its LoginDto - but our
// own class: a client copies the shape, it does not reference the other API.
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
