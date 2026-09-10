namespace NotesApp.Domain.Models;

// Configured with the FLUENT API in EntityConfigurationHelper.ConfigureTag().
// A plain C# class: no attributes, no EF Core using.
public class Tag : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}
