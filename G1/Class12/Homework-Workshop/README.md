# Workshop — Movies API 🎬🗄️

Today you build a **complete Movies API on your own** —
layered, backed by **SQL Server through Entity Framework Core**.

This document tells you *what* to build and *what it must do*. It does not tell you how —
that part is your job.

---

## 🚀 What you are building

A Web API that manages a movie catalogue: **movies, genres, directors and actors**, stored in a
**real SQL Server database** you create yourself with EF Core migrations.


What is being assessed:

| Concept | Where it shows up today |
|---|---|
| N-Tier architecture | the solution has layers, and they respect each other |
| EF Core Code First | your C# classes create the database |
| **Fluent API** | every entity is configured in `OnModelCreating` — **no Data Annotations on entities** |
| Relationships | 1:M (Genre→Movie, Director→Movie) and M:M (Movie↔Actor) |
| Migrations | `Init` + `SeedData`, applied to a database you can open in SSMS |
| Complex queries | `Include` / `ThenInclude`, filtering in the database |
| Controllers & routing | four controllers, route templates, sub-resources |
| Model binding | `[FromBody]`, `[FromQuery]`, `[FromRoute]` |
| DTOs | an entity never reaches the client, a client DTO never reaches EF |
| Validation | request shape *and* business rules, each in its own layer |
| Status codes | 200, 201, 204, 400, 404, 409 |
| **⭐ Bonus: Dapper** | every **read** goes through Dapper, every **write** stays on EF Core |

---

## ✏ The project

- **.NET 8.** Not .NET 9, not .NET 10.

  > **In Visual Studio:** the Framework dropdown must read **.NET 8.0 (Long Term Support)**
  > and **"Use controllers"** must be ticked. Check both before clicking Create.

- **Controllers, not Minimal APIs.**
- Delete the `WeatherForecast` model and controller — template noise.
- Confirm Swagger opens at `/swagger` **before** you write a single line of your own code.
- Solution name: `MoviesApp`. Project names follow the `MoviesApp.*` pattern.

### Packages

Exactly the ones we used in the Notes App. Nothing newer — a `9.x` Microsoft package on a
`net8.0` project will fight you.

| Package | Version | Goes in |
|---|---|---|
| `Microsoft.EntityFrameworkCore.SqlServer` | `8.0.30` | Data Access |
| `Microsoft.EntityFrameworkCore.Tools` | `8.0.30` | Data Access |
| `Microsoft.EntityFrameworkCore.Design` | `8.0.30` | Data Access **and** API |
| `Dapper` | `2.1.79` | Data Access — **bonus only** |

---

## 🏗️ Architecture (N-Tier)

This is not a one-file workshop. **The solution must be split into layers, the same way we split
the Notes App.** Separate class library projects, not folders.

At minimum you need projects for:

- the **API** — controllers and `Program.cs`
- the **Services** — everything the application decides
- the **Data Access** — the `DbContext`, the entity configuration, the migrations, the repositories
- the **Domain** — the entity classes
- the **DTOs** — the shapes the outside world sees
- mapping between entities and DTOs (its own project, or inside the layer you can defend)

Rules that will be checked:

> The controller contains **no business rules**: no validation logic, no searching, no `Include()`.
> It translates HTTP into a service call and back.
>
> The service **never mentions `IActionResult`, `Ok()`, `NotFound()` or a status code**. It must be
> callable from a console app tomorrow without changing a line.
>
> **Only the Data Access layer knows that EF Core exists.** If `using Microsoft.EntityFrameworkCore;`
> or the word `DbContext` appears in a controller or a service, the layer boundary is broken.
>
> The Domain project references **nothing** — not even EF Core. That is what makes the Fluent API
> requirement below possible.
>
> Dependencies go **one way only**, and every dependency is an **interface** resolved by DI.
> No `new SomeService()` or `new SomeRepository()` anywhere outside `Program.cs`.
>
> The service reports failure by **throwing** a custom exception, not by returning null and hoping.
> The controller decides which status code that failure becomes.

Draw the layer diagram on paper before you create the projects. If you cannot draw it, you cannot
build it.

---

## 🎬 The domain

Four entities. The genre is **no longer an enum** — it is a table, because a genre is data the
customer wants to add to without a redeploy.

```text
   Genre  1 ────< M  Movie  M >──── 1  Director
                      │
                      M
                      │   (MovieActor — join table)
                      M
                    Actor
```

### Movie

| Field | Type | Notes |
|---|---|---|
| Id | int | assigned by the database, never by the client |
| Title | string | required, max 200 |
| Description | string? | optional, max 1000 |
| Year | int | required |
| DurationMinutes | int | required |
| GenreId | int | required FK → Genre |
| DirectorId | int? | optional FK → Director |
| Actors | List&lt;Actor&gt; | many-to-many |

### Genre

| Field | Type | Notes |
|---|---|---|
| Id | int | |
| Name | string | required, max 50, **unique** |
| Movies | List&lt;Movie&gt; | |

### Director

| Field | Type | Notes |
|---|---|---|
| Id | int | |
| FirstName | string | required, max 50 |
| LastName | string | required, max 50 |
| DateOfBirth | DateTime? | optional, stored as `date` — not `datetime2` |
| Movies | List&lt;Movie&gt; | |

### Actor

| Field | Type | Notes |
|---|---|---|
| Id | int | |
| FirstName | string | required, max 50 |
| LastName | string | required, max 50 |
| Movies | List&lt;Movie&gt; | many-to-many |

> A `BaseEntity` with `Id`, `CreatedDate` and `UpdatedDate` is a good idea — you saw one in the
> Notes App. Use it or don't, but be able to say why.

---

## 🔧 Fluent API — the hard requirement

**Every one of the four entities is configured with the Fluent API, in `OnModelCreating`.**

> Your entity classes must be plain C#: no `[Required]`, no `[MaxLength]`, no `[Table]`, no
> `[ForeignKey]`, and **no `using Microsoft.EntityFrameworkCore;`** in the Domain project.
> If your Domain project compiles without a single EF Core reference, you did it right.

Keep `OnModelCreating` readable — one extension method per entity, in a configuration helper
(`modelBuilder.ConfigureMovie();`), or one `IEntityTypeConfiguration<T>` class per entity. Both are
fine. A 200-line `OnModelCreating` is not.

You must configure, at minimum:

- **Table names** — singular, explicit: `Movie`, `Genre`, `Director`, `Actor`, `MovieActor`
- **`IsRequired()` and `HasMaxLength()`** on every string, matching the tables above
- **`HasColumnType("date")`** on `Director.DateOfBirth`
- **A unique index** on `Genre.Name`
- **The two 1:M relationships**, each with a deliberate `OnDelete` behaviour:
  - deleting a **Genre** that still has movies must **not** silently delete the movies
  - deleting a **Director** must leave their movies in place, with no director
  - the two rules above are not the same `DeleteBehavior`. Work out which is which.
- **The M:M relationship**, through a join table you name yourself (`MovieActor`), with columns
  called `MovieId` and `ActorId` — not `ActorsId`
- **An index** on the columns you filter by most (`Movie.GenreId`, `Movie.Year`)

### Migrations

Two migrations, both committed:

* Init    
* SeedData  

(In the Package Manager Console: `Add-Migration Init`, with **Default project = DataAccess** and
`MoviesApp` set as the startup project.)

Seed with `HasData` inside `OnModelCreating`, exactly as we did for the Notes App:

- **at least 4 genres**, **3 directors**, **6 actors**
- **at least 8 movies**, spread over **3+ genres** and **3+ years**, at least one with **no director**
- **at least 10 rows in the join table**, including one movie with **three or more actors** and one
  movie with **none**

If every movie is a 2024 comedy with one actor, you cannot tell a working filter from a broken one.

> ⚠ `HasData` needs **explicit ids** on everything, including the join-table rows. It also refuses
> to seed anything with a `DateTime.UtcNow` in it — the value has to be constant, or every
> `Add-Migration` produces a new one. Find out why before you fight it.

Open the database in **SSMS** before you write a controller. Five tables, the foreign keys where
you expect them, and rows in all of them. **Do not continue until you have seen it.**

---

## 📦 DTOs

The client never sees an entity — not on the way in, not on the way out.

You need, at minimum: a **read** DTO and a **create/update** DTO for `Movie`, plus small read DTOs
for `Genre`, `Director` and `Actor`.

---

## ✅ Validation

Three kinds of rule now, and part of the exercise is deciding **which layer each one belongs to**:

**Request shape** — the DTO knows this

- Title is required, maximum 200 characters
- Description is optional; if provided, maximum 1000 characters
- Year must be in a sensible range — pick it and defend it
- DurationMinutes must be greater than 0

**Business rules** — the service knows this

- Creating or updating a movie with a `GenreId` that does not exist must fail as **400**, not 500
- Same for a `DirectorId` and for every id in `ActorIds`
- Updating or deleting a movie that does not exist must fail as **404**
- A genre name must be unique — a duplicate is **409 Conflict**, not 400 and not a SQL exception
- *(stretch)* two movies may not have the same title in the same year

**Database rules** — the Fluent API knows this

- The same lengths and required-ness as above, so the column really is `nvarchar(200) NOT NULL`

> ⚠ Yes, "Title is required, max 200" is now stated in **three** places: the DTO, the service and
> `OnModelCreating`. That is not duplication for its own sake — each one fails at a different
> moment, with a different error, for a different audience. Be ready to say what happens if you
> delete each one.
>
> ⚠ And the trap from the earlier classes is still here: `[Required]` behaves differently on a
> `string` than on an `int`. Find out what an omitted `year` and an omitted `genreId` actually
> arrive as. If your API accepts `{"title":"X"}` and stores year `0` with `GenreId` `0`, you have
> not finished this requirement.

An invalid request must come back as **400 with a body that names the offending field**.

---

## 🔀 The endpoints

These tables are the contract — your API is correct when it matches them exactly.

### Movies

| # | Method | Route | Body | Success | Failure |
|---|---|---|---|---|---|
| 1 | GET | `/api/movies` | — | 200 + list | 500 |
| 2 | GET | `/api/movies?genreId=&year=&title=` | — | 200 + filtered list | 400 |
| 3 | GET | `/api/movies/{id}` | — | 200 + one movie, with genre, director and actors | 404 |
| 4 | POST | `/api/movies` | create DTO | 201 + `Location` header | 400 |
| 5 | PUT | `/api/movies/{id}` | update DTO | 204 | 400, 404 |
| 6 | DELETE | `/api/movies/{id}` | — | 204 | 404 |

## Bonus endpoints 

### Genres

| # | Method | Route | Body | Success | Failure |
|---|---|---|---|---|---|
| 7 | GET | `/api/genres` | — | 200 + list, each with its movie **count** | 500 |
| 8 | POST | `/api/genres` | create DTO | 201 + `Location` header | 400, **409** if the name exists |
| 9 | DELETE | `/api/genres/{id}` | — | 204 | 404, **409** if movies still use it |

### Directors

| # | Method | Route | Body | Success | Failure |
|---|---|---|---|---|---|
| 10 | GET | `/api/directors` | — | 200 + list | 500 |
| 11 | GET | `/api/directors/{id}` | — | 200 + director **with their movies** | 404 |
| 12 | POST | `/api/directors` | create DTO | 201 + `Location` header | 400 |

### Actors

| # | Method | Route | Body | Success | Failure |
|---|---|---|---|---|---|
| 13 | GET | `/api/actors?movieId=` | — | 200 + list | 400 |
| 14 | POST | `/api/actors` | create DTO | 201 + `Location` header | 400 |
| 15 | POST | `/api/movies/{movieId}/actors/{actorId}` | — | 204 | 404, 409 if already cast |
| 16 | DELETE | `/api/movies/{movieId}/actors/{actorId}` | — | 204 | 404 |


### Filtering is the database's job

`GET /api/movies?genreId=2&year=1994` must produce **one SQL query with a `WHERE` clause**. Loading
every movie into memory and then calling `.Where()` on the `List<Movie>` is wrong, and it is the
single most common mistake in this workshop.

> Build the query as `IQueryable`, add each filter only if the parameter has a value, and call
> `ToListAsync()` **once**, at the end. Watch the SQL EF Core generates in the console window — if
> the `WHERE` is not in there, you materialised too early.

---

## 🔽 Status codes

| Code | Meaning | Return it when |
|---|---|---|
| 200 OK | here is your data | a GET succeeded |
| 201 Created | made it, here is where | a POST succeeded |
| 204 No Content | done, nothing to say | an update or delete succeeded |
| 400 Bad Request | *your* request is wrong | validation failed, or a referenced id does not exist |
| 404 Not Found | no such thing | the id in the route does not exist |
| 409 Conflict | it clashes with what is already there | duplicate genre name, actor already cast |
| 500 Server Error | *our* mistake | something you did not anticipate |

Requirements:

- **Every error your API returns has the same JSON shape.** `[ApiController]` already produces
  `application/problem+json` for the validation errors it catches on its own — match it instead of
  inventing a second shape. `ControllerBase` gives you `Problem(statusCode:, title:, detail:)`
  for exactly this. **No hand-rolled `new { StatusCode, Message }` envelopes.**
- **Successes return the resource itself** — `Ok(movieDto)`, or `CreatedAtAction(...)` for a 201
  with its `Location` header. No wrapper object around it.
- Type your actions `ActionResult<T>` so Swagger documents what comes back.
- **Never send an exception message from an unexpected error to the client.** A `SqlException`
  message tells an attacker your server name, your database name and your schema.

---

## ⭐ Bonus — reads on Dapper, writes on EF Core

Only start this when everything above works. It is the most realistic thing in this document:
plenty of production systems write through an ORM and read through hand-written SQL.

**The rule:** every **read** (`GET`) reaches the database through **Dapper**. Every **write**
(`POST`, `PUT`, `DELETE`) still goes through **EF Core**. The service layer must not be able to
tell — **not one line above the Data Access layer may change.**

That constraint is the whole exercise. To satisfy it you will have to split your repository
interface in two:

```csharp
public interface IMovieReadRepository   // Dapper
{
    Task<List<Movie>> GetAllAsync(int? genreId, int? year, string? title);
    Task<Movie?> GetByIdAsync(int id);
}

public interface IMovieWriteRepository  // EF Core
{
    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(Movie movie);
}
```

Both are registered in `Program.cs`; the service takes both in its constructor.

What you will run into, and must handle:

- **The filter belongs in the SQL now.** Build the `WHERE` clause from the parameters that have a
  value, and pass the values as **Dapper parameters** (`@GenreId`), never by string concatenation.
  One `'; DROP TABLE Movie--` in the `title` parameter is the demo nobody wants to give.
- **The genre, the director and the actors have to be joined in by hand.** `Include()` does not
  exist here. `QueryAsync<Movie, Genre, Director, Movie>(..., splitOn: "Id,Id")` plus a dictionary
  keyed by movie id is the pattern — the join repeats a movie once per actor, and collapsing those
  rows is your job now, not the ORM's.
- **A movie that Dapper loaded is not tracked by EF Core.** So `PUT` cannot read with Dapper and
  save with EF — the write repository has to load its own copy through the `DbContext` first. Find
  this out by trying it; the failure is quiet, not loud.
- **Column names must match property names** for Dapper's mapping to work, which is exactly why the
  Fluent API section above made you name your tables and columns deliberately.

Sanity check when you are done: comment out the Dapper registration in `Program.cs`, put an EF read
repository back in its place, and **every request in your Postman collection must still pass,
unchanged**. If it doesn't, the abstraction leaked.

---

## ✅ Definition of Done

Your API is finished when every one of these passes. Test them, do not assume them:

**Database**

- [ ] `dotnet build` is clean — **zero warnings**, not just zero errors
- [ ] The Domain project has **no reference to EF Core** and no attributes on the entities
- [ ] `Init` and `SeedData` migrations exist, and `dotnet ef database update` runs on a clean machine
- [ ] SSMS shows 5 tables, the right foreign keys, and seeded rows in all of them
- [ ] `Genre.Name` has a **unique index** — inserting a duplicate in SSMS fails
- [ ] `Director.DateOfBirth` is a `date` column, not `datetime2`
- [ ] The join table is called `MovieActor`, with columns `MovieId` and `ActorId`

**Architecture**

- [ ] Separate project per layer, and Domain references nothing
- [ ] `DbContext` appears in exactly one project
- [ ] No `new` on any service, repository or `DbContext` outside `Program.cs`
- [ ] No entity crosses the controller boundary — DTOs only

**Behaviour**

- [ ] Every endpoint behaves exactly as the contract tables say
- [ ] `GET /api/movies/999` → 404 — not 500, not an empty 200
- [ ] `GET /api/movies/{id}` shows genre name, director name and actor names — not ids
- [ ] `GET /api/movies?genreId=2&year=1994` filters **in SQL** (you checked the generated query)
- [ ] A filter that matches nothing → the answer you decided on, deliberately
- [ ] `POST` a movie with an empty title → 400, and the body names `Title`
- [ ] `POST` a movie with `genreId: 999` → 400, and the message says which id was wrong
- [ ] `POST` a movie with no `year` and no `genreId` → 400
- [ ] `POST` returns 201 **and** a `Location` header that actually resolves
- [ ] `POST /api/genres` with an existing name → 409
- [ ] `DELETE /api/genres/{id}` on a genre that still has movies → 409, and **nothing is deleted**
- [ ] Casting the same actor twice → 409, and the join table has one row, not two
- [ ] `PUT` then `GET` shows the new values, including changed actors
- [ ] `DELETE` then `GET` on the same id → 404
- [ ] Every error response has the same `application/problem+json` shape
- [ ] ⭐ Every `GET` runs on Dapper, every write on EF Core, and the service layer never found out

---

## 📕 Testing

Test in **Swagger** as you go, then again in **Postman**, and **look at the database in SSMS after
every write** — a 204 only proves that your controller returned 204.

Watch the **status code and the response headers**, not just the body. Half the checklist above is
invisible if you only read the JSON.

Two things worth having open all class:

- the **console window** running your API — EF Core logs every SQL statement it generates there
- **SSMS**, on `MoviesAppDb`

The `.http` file that shipped with the template works too, straight inside Visual Studio.

---

## 🚀 Stretch goals

Finished early, including the bonus? In this order:

1. **Paging:** `GET /api/movies?page=1&pageSize=10`, with the total count in a response header
2. **Sorting:** `GET /api/movies?sortBy=year&desc=true` — without opening a SQL injection hole
3. **`GET /api/movies/{id}/similar`** — same genre, released within 5 years, excluding itself
4. **`GET /api/actors/{id}/co-stars`** — everyone who has been in a movie with this actor
5. **`AsNoTracking()`** on every read — measure whether it changed anything, and say why
6. **A stored procedure** for the movie search, called from Dapper

---

## 🤖 Let's Ask AI

Use AI to understand the concepts, **not** to produce the solution. You will be asked to explain
your own code out loud, line by line, and "Copilot wrote it" is not an explanation.

### Good prompts

```text
Explain the difference between DeleteBehavior.Cascade, Restrict, SetNull and NoAction, with an example of each.
```

```text
Why does EF Core name my many-to-many join columns "ActorsId" by default, and how do I control that with the Fluent API?
```

```text
I called .ToList() before .Where(). Explain what SQL EF Core generates in that case versus the other way around.
```

```text
What are the rules and limitations of HasData seeding in EF Core 8?
```

```text
Explain Include and ThenInclude, and when eager loading becomes a performance problem.
```

```text
What does splitOn do in Dapper's multi-mapping, and why is my second object null?
```

```text
Why is a Dapper-loaded entity not tracked by EF Core, and what breaks because of that?
```

```text
Review my OnModelCreating and tell me which configuration is missing - do not write the code.
```

```text
Review my project references and tell me if my layering is violated anywhere.
```

```text
Should a duplicate genre name be 400 or 409? Argue both sides.
```

### Avoid prompts like

```text
Generate the entire Movies API.
```

```text
Write my MoviesController and my DbContext for me.
```

```text
Implement the complete workshop.
```

The goal today is to learn **how to build the API**. Code you did not write is code you cannot
debug — and today, code you cannot debug is code whose migration you cannot fix.
