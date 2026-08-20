# Building the Note API - Layer by Layer 🏗️

Until now everything we wrote lived in one place: the controller. It read the data,
it decided what was valid, it built the response. It worked - and it worked because
our app was tiny.

Today we split that one file into **layers**. By the end of this class the Notes API
will have the same shape that real backend projects have.

---

## What is N-Tier architecture? 🔸

**N-Tier** simply means: *split the application into layers, where each layer has one
job and only talks to the layer directly below it.*

```text
        HTTP request
             |
             v
   ┌───────────────────┐
   │   API (Controller)│  "translate HTTP into a method call"
   └─────────┬─────────┘
             v
   ┌───────────────────┐
   │      Services     │  "the rules of our application"
   └─────────┬─────────┘
             v
   ┌───────────────────┐
   │    Data Access    │  "get me the data, save the data"
   └─────────┬─────────┘
             v
   ┌───────────────────┐
   │  StaticDb (today) │  → a real SQL database further on
   └───────────────────┘

   Domain / Dtos / Mappers are used BY these layers, they are not a step in the chain.
```

The rule that makes this work is boring but absolute:

> **Arrows point in one direction only.**
> The controller knows about the service. The service does **not** know the controller exists.

### Why bother? 🔽

- **You can change one layer without touching the others.** 
- **You can test the rules without starting a web server** 
- **Two people can work at once** - one on the controller, one on the service.
- **You always know where to look.** Bug in a status code? Controller. Wrong validation message? Service. Wrong data? Repository.

---

## Our solution 🔸

Six projects instead of one. In Visual Studio: right click the solution → Add → New Project → Class Library.

| Project | Contains | References |
|---|---|---|
| `NotesApp` | Controllers, `Program.cs` | Services, Dtos, Domain |
| `NotesApp.Services` | `INoteService`, `NoteService`, custom exceptions | Domain, Dtos, DataAccess, Mappers |
| `NotesApp.Mappers` | `NoteMapper`, `TagMapper` | Domain, Dtos |
| `NotesApp.DataAccess` | `StaticDb`, `IRepository<T>`, repositories | Domain |
| `NotesApp.Dtos` | `NoteDto`, `AddNoteDto`, `UpdateNoteDto`, `TagDto` | Domain |
| `NotesApp.Domain` | `Note`, `Tag`, `User`, `Priority` | *(nothing)* |

Look at the last row. **The Domain project references nothing.** It is the centre of the
application and it does not know that HTTP, JSON or SQL exist. That is a good sign.

### 🤖 Let's Ask AI

```text
Explain N-Tier architecture using a restaurant as an analogy.
```

```text
What would break if my DataAccess project referenced my API project?
```

---

## Domain models 🔸

The domain model is the note **as our application thinks about it**.

```csharp
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

public class Note : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public Priority Priority { get; set; }

    public int UserId { get; set; }      // foreign key
    public User? User { get; set; }      // navigation property

    public List<Tag> Tags { get; set; } = new List<Tag>();
}
```

---

## DTOs - Data Transfer Objects 🔸

A DTO is the shape of the data **as it travels over HTTP**. It is our contract with
whoever is calling us.

```csharp
public class NoteDto            // what we SEND BACK
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public string UserFullName { get; set; } = string.Empty;   // flattened!
    public List<TagDto> Tags { get; set; } = new List<TagDto>();
}

public class AddNoteDto         // what we RECEIVE when creating
{
    public string Text { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public int UserId { get; set; }
    public List<int> TagIds { get; set; } = new List<int>();
}
```

### Why not just return the domain model? 🔽

Open `Domain/Models/User.cs` and look at line with `Password`.

If a controller returns a `Note`, and that `Note` has a `User`, and that `User` has a
`Password` - then **you just published every password in your database as JSON.**
Nobody wrote the bug. It happened because the model was reused.

Other reasons:

- **The client does not need everything.** Why send `UserId`, `User.Id`, `User.Username`,
  `User.Notes[...]` when the screen shows "Ana Ilievska"?
- **Different operations need different data.** Creating a note has no `Id`.
  Updating one does. One class cannot honestly describe both.
- **Your API stays stable.** Rename a property in the domain model and the JSON your
  clients depend on does not change - only the mapper does.

> `AddNoteDto` and `UpdateNoteDto` look almost identical today. Resist merging them.
> The moment one of them gets a rule the other does not have, you would have to split
> them again - and by then three endpoints depend on the shared class.

### 🤖 Let's Ask AI

```text
Show me a real security incident caused by returning database entities directly from an API.
```

```text
What is over-posting / mass assignment, and how do DTOs prevent it?
```

---

## The Mapper layer 🔸

Something has to turn a `Note` into a `NoteDto`. That something is the mapper.
We write it **by hand** - as extension methods, so the call reads nicely.

```csharp
public static class NoteMapper
{
    public static NoteDto ToNoteDto(this Note note)
    {
        return new NoteDto
        {
            Id = note.Id,
            Text = note.Text,
            Priority = note.Priority,
            UserFullName = note.User is null
                ? "Unknown"
                : $"{note.User.FirstName} {note.User.LastName}",
            Tags = note.Tags.ToTagDtoList()
        };
    }

    public static Note ToNote(this AddNoteDto addNoteDto) { /* ... */ }

    public static void ApplyTo(this UpdateNoteDto dto, Note existingNote) { /* ... */ }
}
```

Now the service can write `note.ToNoteDto()` instead of twelve lines of assignment.

Two things worth noticing:

- **The mapper never touches the database.** It only moves values between objects.
  Turning `TagIds = [2, 5]` into real `Tag` objects requires *looking them up* - that is
  a decision, and decisions belong in the service.
- **`ApplyTo` does not create a new `Note`.** It copies values onto the note we already
  read. 

> There are libraries that generate mappers for you - AutoMapper, Mapperly. We write it
> by hand first so that when you meet one of them, you know exactly what it replaced.

---

## The Data Access layer 🔸

One interface, one job:

```csharp
public interface IRepository<T> where T : BaseEntity
{
    List<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
```

And an implementation that talks to our fake database:

```csharp
public class NoteRepository : IRepository<Note>
{
    public Note? GetById(int id)
    {
        return StaticDb.Notes.FirstOrDefault(note => note.Id == id);
    }

    public void Add(Note entity)
    {
        entity.Id = StaticDb.NextNoteId();   // a real database does this for us
        StaticDb.Notes.Add(entity);
    }
    // ...
}
```

`GetById` returns `Note?` - it can return `null`, and it does **not** throw.
"I did not find it" is a fact. "That is a problem" is an opinion, and opinions
belong one layer up.

---

## The Service layer 🔸

The service is where the application actually thinks. Every method follows the same
three beats:

```csharp
public NoteDto AddNote(AddNoteDto addNoteDto)
{
    // 1. validate
    ValidateText(addNoteDto.Text);
    ValidatePriority(addNoteDto.Priority);
    User user = GetUserOrThrow(addNoteDto.UserId);
    List<Tag> tags = GetTagsOrThrow(addNoteDto.TagIds);

    // 2. map
    Note newNote = addNoteDto.ToNote();
    newNote.User = user;
    newNote.Tags = tags;

    // 3. save
    _noteRepository.Add(newNote);

    return newNote.ToNoteDto();
}
```

There is no `IActionResult` here, no status code, no `[FromBody]`. The service does not
know it is being called by a web API. Tomorrow a console application could call it.

### How does the service report a problem? 🔽

It cannot return `NotFound()` - that is an HTTP idea. So it throws:

```csharp
public class NoteNotFoundException : Exception { /* ... */ }   // controller -> 404
public class NoteDataException : Exception     { /* ... */ }   // controller -> 400
```

Two exception types, two status codes. The service describes *what went wrong*;
the controller decides *how to say it in HTTP*.

### Dependency Injection 🔽

Look at the constructor:

```csharp
public NoteService(
    IRepository<Note> noteRepository,
    IRepository<User> userRepository,
    IRepository<Tag> tagRepository)
```

The service asks for **interfaces**. It never writes `new NoteRepository()`.
Someone else decides which class to hand it - and that someone is `Program.cs`:

```csharp
builder.Services.AddScoped<IRepository<Note>, NoteRepository>();
builder.Services.AddScoped<IRepository<Tag>, TagRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<INoteService, NoteService>();
```

Read it as a sentence: *"whenever somebody asks for `INoteService`, give them a `NoteService`."*

This is why we can swap the static store for a real database later, and it is
why we can hand the service a **fake** repository to test it.

### 🤖 Let's Ask AI

```text
Explain the difference between AddScoped, AddTransient and AddSingleton with an example.
```

```text
What is the Dependency Inversion Principle and how does IRepository demonstrate it?
```

---

## The Controller 🔸

After all that, the controller becomes small - and that is the point.

```csharp
[HttpGet("{id}")]
public ActionResult<NoteDto> GetById(int id)
{
    try
    {
        return Ok(_noteService.GetNoteById(id));
    }
    catch (NoteNotFoundException e)
    {
        return NotFound(e.Message);
    }
    catch (Exception)
    {
        return StatusCode(StatusCodes.Status500InternalServerError,
            "An error occurred, please contact the administrator.");
    }
}
```

> Notice the last `catch`. We send the client a generic message and we never leak
> `e.Message` from an unexpected exception - a stack trace tells an attacker about your
> database.

### Our endpoints

| Method | Route | Success | Failure |
|---|---|---|---|
| GET | `/api/notes` | 200 | 500 |
| GET | `/api/notes?priority=High` | 200 | 500 |
| GET | `/api/notes/{id}` | 200 | 404 |
| POST | `/api/notes` | 201 + `Location` header | 400 |
| PUT | `/api/notes/{id}` | 204 | 400, 404 |
| DELETE | `/api/notes/{id}` | 204 | 404 |

Two details worth stealing for your own APIs:

- **`CreatedAtAction`** returns 201 *and* a `Location: /api/Notes/4` header telling the
  client where the new note lives. Try it in Swagger and look at the response headers.
- **`PUT /api/notes/{id}` with an `id` in the body too.** If the two disagree the request
  is ambiguous, so we answer 400 instead of guessing.

---

### 🤖 Let's Ask AI

```text
Review my TagService and tell me if any business logic leaked into the controller.
```

```text
Which HTTP status code should I return when deleting something that is still in use?
```

---

## Summary

- **N-Tier** = layers with one job each, and arrows that point in one direction.
- **Domain models** are ours; **DTOs** are the contract with the outside world.
- **Mappers** translate between them - and nothing else.
- **Repositories** fetch and store; they have no opinions.
- **Services** hold the rules and know nothing about HTTP.
- **Controllers** translate HTTP in, and status codes out.
- **Dependency Injection** in `Program.cs` is the glue that lets us swap any layer.
