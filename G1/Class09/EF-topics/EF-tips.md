# 20 Common Entity Framework Core Mistakes (and How to Fix Them)

A practical checklist for EF Core. Examples target EF Core 8/9, but most points apply to EF6 as well.

---

## 1. The N+1 query problem

**Mistake**

```csharp
var orders = await db.Orders.ToListAsync();
foreach (var order in orders)
{
    // Lazy loading fires one query per order
    Console.WriteLine(order.Customer.Name);
}
```

**Fix** — load related data up front:

```csharp
var orders = await db.Orders
    .Include(o => o.Customer)
    .ToListAsync();
```

For collections that would explode the result set, use split queries:

```csharp
var orders = await db.Orders
    .Include(o => o.Lines)
    .Include(o => o.Customer)
    .AsSplitQuery()
    .ToListAsync();
```

**Tip:** enable a logging warning so N+1 shows up in development. Log at `Information` level and watch how many `SELECT` statements a single request produces.

---

## 2. Fetching entities when you only need a few columns

**Mistake**

```csharp
var users = await db.Users.ToListAsync();
var names = users.Select(u => u.FullName).ToList();
```

This pulls every column of every row, materializes full entities, and adds them to the change tracker.

**Fix** — project into a DTO or anonymous type so the SQL only selects what you need:

```csharp
var names = await db.Users
    .Select(u => new UserListItem(u.Id, u.FullName))
    .ToListAsync();
```

Projections are also automatically no-tracking.

---

## 3. Not using `AsNoTracking` for read-only queries

Change tracking costs memory and CPU: EF snapshots every entity so it can detect modifications you never intend to make.

```csharp
var products = await db.Products
    .AsNoTracking()
    .Where(p => p.IsActive)
    .ToListAsync();
```

For an entire read-only context:

```csharp
options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
```

Use `AsNoTrackingWithIdentityResolution()` when the same entity appears many times in the graph and you need reference equality.

---

## 4. Accidental client-side evaluation

**Mistake**

```csharp
var results = await db.Users
    .Where(u => MyHelper.Normalize(u.Email) == input)  // can't translate
    .ToListAsync();
```

EF Core 3.0+ throws instead of silently pulling the whole table — but people "fix" it by adding `.ToList()` or `.AsEnumerable()` before the filter, which downloads the table anyway.

**Fix** — express the filter in something EF can translate, or push the logic into the database (computed column, `EF.Functions`, or a mapped SQL function):

```csharp
.Where(u => EF.Functions.Like(u.Email, $"%{domain}"))
```

---

## 5. Calling `Where` after `ToList`

```csharp
// Bad: filters in memory after downloading everything
var recent = (await db.Orders.ToListAsync())
    .Where(o => o.CreatedAt > cutoff);

// Good: filters in SQL
var recent = await db.Orders
    .Where(o => o.CreatedAt > cutoff)
    .ToListAsync();
```

Rule of thumb: every `ToList`, `ToArray`, `AsEnumerable`, or `foreach` is the point where LINQ-to-Entities becomes LINQ-to-Objects. Do all filtering, ordering, and paging before it.

---

## 6. Paging without ordering

`Skip`/`Take` without `OrderBy` gives an undefined row order — pages can repeat or drop rows.

```csharp
var page = await db.Orders
    .OrderByDescending(o => o.CreatedAt)
    .ThenBy(o => o.Id)               // tiebreaker keeps it deterministic
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

For deep paging, prefer keyset pagination (`WHERE (CreatedAt, Id) < (@lastDate, @lastId)`) over large `Skip` values, which force the database to scan and discard rows.

---

## 7. Wrong DbContext lifetime

`DbContext` is **not** thread-safe and is designed as a short-lived unit of work.

- Registering it as a singleton causes concurrency exceptions and unbounded memory growth from the change tracker.
- Injecting a scoped context into a singleton service (or a background service) captures a disposed context.

```csharp
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(cs)); // Scoped by default
```

In singletons, background services, and Blazor Server, use a factory:

```csharp
builder.Services.AddDbContextFactory<AppDbContext>(o => o.UseSqlServer(cs));

await using var db = await factory.CreateDbContextAsync();
```

---

## 8. Mixing sync and async, or blocking on async

```csharp
var user = db.Users.FirstAsync(...).Result;  // deadlock / thread-pool starvation
```

Pick async and stay async all the way up: `ToListAsync`, `FirstOrDefaultAsync`, `AnyAsync`, `SaveChangesAsync`, and pass a `CancellationToken` so abandoned requests release the connection.

---

## 9. Using `Count() > 0` instead of `Any()`

```csharp
// Bad: SELECT COUNT(*) ...
if (await db.Orders.CountAsync(o => o.UserId == id) > 0)

// Good: SELECT TOP(1) 1 ... — stops at the first match
if (await db.Orders.AnyAsync(o => o.UserId == id))
```

Same idea: use `AnyAsync` instead of `FirstOrDefaultAsync(...) != null` when you only need existence.

---

## 10. `SaveChanges` inside a loop

```csharp
// Bad: one round trip per row
foreach (var item in items)
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
}

// Good: one batched round trip
db.Items.AddRange(items);
await db.SaveChangesAsync();
```

For bulk updates and deletes, EF Core 7+ can do it entirely in SQL without loading entities:

```csharp
await db.Orders
    .Where(o => o.CreatedAt < cutoff)
    .ExecuteDeleteAsync();

await db.Products
    .Where(p => p.CategoryId == id)
    .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsActive, false));
```

Note that `ExecuteUpdate`/`ExecuteDelete` bypass the change tracker and don't fire `SaveChanges` interceptors.

---

## 11. Loading thousands of entities just to modify or delete them

Loading 50,000 rows into memory to flip one boolean is slow and memory-hungry. Use `ExecuteUpdateAsync`/`ExecuteDeleteAsync` (above), a bulk-extension library, or raw SQL.

If you must go through the change tracker for very large batches, chunk the work and use a fresh context per chunk so the tracker doesn't grow without bound.

---

## 12. Ignoring transactions across multiple `SaveChanges` calls

A single `SaveChanges` is already transactional. Multiple calls are not.

```csharp
await using var tx = await db.Database.BeginTransactionAsync();
try
{
    await db.SaveChangesAsync();
    await someOtherService.DoWorkAsync();
    await db.SaveChangesAsync();
    await tx.CommitAsync();
}
catch
{
    await tx.RollbackAsync();
    throw;
}
```

If you enable retrying execution strategies (recommended for cloud databases), wrap manual transactions in the strategy:

```csharp
var strategy = db.Database.CreateExecutionStrategy();
await strategy.ExecuteAsync(async () => { /* transaction here */ });
```

---

## 13. No concurrency control

Last-write-wins silently destroys data. Add a concurrency token:

```csharp
public class Product
{
    public int Id { get; set; }
    public decimal Price { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = default!;
}
```

Then handle the conflict:

```csharp
try { await db.SaveChangesAsync(); }
catch (DbUpdateConcurrencyException ex) { /* reload, merge, or surface to user */ }
```

For PostgreSQL, `xmin` works as the token: `Property(p => p.Version).IsRowVersion()` via the Npgsql provider.

---

## 14. String concatenation in raw SQL

```csharp
// SQL injection
db.Users.FromSqlRaw($"SELECT * FROM Users WHERE Name = '{name}'");

// Safe: interpolation is parameterized
db.Users.FromSql($"SELECT * FROM Users WHERE Name = {name}");

// Or explicit parameters
db.Users.FromSqlRaw("SELECT * FROM Users WHERE Name = {0}", name);
```

`FromSql` / `FromSqlInterpolated` turn interpolated values into `DbParameter`s. `FromSqlRaw` does not.

---

## 15. Missing or badly designed indexes

EF creates indexes for foreign keys but knows nothing about your query patterns. Add them explicitly:

```csharp
modelBuilder.Entity<Order>()
    .HasIndex(o => new { o.CustomerId, o.CreatedAt })
    .HasDatabaseName("IX_Orders_Customer_CreatedAt");

modelBuilder.Entity<User>()
    .HasIndex(u => u.Email)
    .IsUnique();
```

Column order in a composite index matters: equality columns first, range/sort columns last. Check the actual execution plan rather than guessing.

---

## 16. Auto-generated migrations applied without review

Generated migrations can drop columns, rename tables destructively, or rebuild large tables while holding locks.

- Always read the generated `Up`/`Down` methods before committing.
- Never call `Database.EnsureCreated()` in an app that also uses migrations — they're mutually exclusive.
- Avoid `db.Database.Migrate()` at startup in multi-instance deployments; two instances can race. Run migrations as a separate deployment step, or generate an idempotent script:

```bash
dotnet ef migrations script --idempotent --output migrate.sql
```

- Rename a property with `RenameColumn`, not drop-and-add, or you lose the data.

---

## 17. Wrong decimal precision for money

SQL Server defaults `decimal` to `decimal(18,2)` in recent EF versions, but silent truncation and precision warnings bite people constantly. Be explicit:

```csharp
modelBuilder.Entity<Order>()
    .Property(o => o.Total)
    .HasPrecision(18, 2);
```

Never use `float`/`double` for currency. And store timestamps as UTC (`DateTimeOffset`, or `DateTime` with a UTC converter) so time zones don't corrupt your data.

---

## 18. Relying on lazy loading in web apps

Lazy loading (`UseLazyLoadingProxies`) makes N+1 invisible and throws `ObjectDisposedException` when a serializer touches a navigation after the context is gone.

Prefer explicit `Include` or projection. If you need on-demand loading in a long-lived context, load explicitly:

```csharp
await db.Entry(order).Collection(o => o.Lines).LoadAsync();
await db.Entry(order).Reference(o => o.Customer).LoadAsync();
```

---

## 19. Returning entities directly from an API

Serializing entities leaks internal columns, drags in navigation properties, and can cause circular-reference errors. It also couples your public contract to your schema, so every table change is a breaking API change.

Project to a DTO in the query itself:

```csharp
app.MapGet("/orders/{id}", async (int id, AppDbContext db) =>
    await db.Orders
        .Where(o => o.Id == id)
        .Select(o => new OrderDto(o.Id, o.Total, o.Customer.Name))
        .FirstOrDefaultAsync());
```

One query, minimal columns, no tracking, stable contract.

---

## 20. Not looking at the generated SQL

Most of the mistakes above are obvious the moment you see the SQL. Turn it on in development:

```csharp
options.UseSqlServer(cs)
       .LogTo(Console.WriteLine, LogLevel.Information)
       .EnableSensitiveDataLogging()   // development only
       .EnableDetailedErrors();        // development only
```

Or inspect a single query:

```csharp
var sql = db.Orders.Where(o => o.Total > 100).ToQueryString();
```

---

## Bonus tips

- **Compiled queries** for hot paths: `EF.CompileAsyncQuery(...)` skips repeated LINQ translation.
- **Global query filters** for soft delete and multi-tenancy: `HasQueryFilter(e => !e.IsDeleted)`; use `IgnoreQueryFilters()` when you deliberately need everything.
- **`Find` vs `FirstOrDefault`**: `FindAsync` checks the change tracker first and skips the round trip if the entity is already loaded.
- **Owned types** (`OwnsOne`) for value objects like `Address` instead of flattening fields by hand.
- **Configure with `IEntityTypeConfiguration<T>`** classes and `ApplyConfigurationsFromAssembly` rather than one enormous `OnModelCreating`.
- **Seed carefully**: `HasData` is for static reference data only; it goes into migrations and needs fixed primary keys.
- **Split queries have a cost**: they issue multiple round trips and aren't consistent without a transaction. Use them for large collection includes, not everywhere.
- **Connection resiliency** for cloud databases: `EnableRetryOnFailure()`.
- **Pool contexts** in high-throughput APIs: `AddDbContextPool<T>` — but not if your context holds per-request state.
- **Benchmark before optimizing.** Measure with real data volumes; EF behavior that's fine at 100 rows can fall apart at 100,000.
