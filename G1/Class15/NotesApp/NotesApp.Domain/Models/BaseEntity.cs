namespace NotesApp.Domain.Models;

// No attributes here either: "Id" is picked up as the key by convention,
// and an int key is an IDENTITY column in SQL Server by convention.
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    protected BaseEntity()
    {
        CreatedDate = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
    }
}
