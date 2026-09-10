namespace NotesApp.Domain.Models;

// Configured with the FLUENT API in EntityConfigurationHelper.ConfigureUser().
// A plain C# class: no attributes, no EF Core using.
public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    // Computed, not stored - the config tells EF Core to Ignore() it.
    public string FullName => $"{FirstName} {LastName}";
    public List<Note> Notes { get; set; } = new();
}
