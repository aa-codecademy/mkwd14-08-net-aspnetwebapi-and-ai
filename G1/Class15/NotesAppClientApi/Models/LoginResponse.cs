namespace NotesAppClientApi.Models;

// What comes back from a successful login.
public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
}
