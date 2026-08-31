# Dapper 🥨

Dapper is one of the most popular micro-ORM libraries for .NET applications.

It was created by the Stack Overflow team and is well known for its simplicity and excellent performance.

Unlike Entity Framework Core, Dapper doesn't provide change tracking or Code First capabilities. Instead, it focuses on executing SQL efficiently while automatically mapping query results to C# objects.

---

# Why Dapper?

Dapper is designed for applications that require high performance and fast database access.

Some of its main benefits include:

- High performance
- Lightweight
- Simple API
- Automatic object mapping
- Stored procedure support
- Multiple query support

Many real-world applications combine Entity Framework Core and Dapper, using EF Core for most operations and Dapper for performance-critical queries.

---

### 🤖 Let's Ask AI

```text
Explain the difference between Dapper and Entity Framework Core.
```

```text
Why is Dapper called a micro ORM?
```

```text
When should I choose Dapper instead of EF Core?
```

---

# Installing Dapper 🔸

Dapper works on top of **ADO.NET**.

For Console Applications or Class Libraries, install:

- `System.Data.SqlClient`
- `Dapper`

.NET Web API projects already include the necessary ADO.NET dependencies.

---

# How Dapper Works 🔸

Unlike Entity Framework Core, Dapper doesn't use a `DbContext`.

Instead, it:

1. Opens a database connection.
2. Executes an SQL query or stored procedure.
3. Maps the returned rows to C# objects.
4. Closes the connection.

Because Dapper maps objects by **name**, class and property names should closely match the database schema.

---

# Example Models

```csharp
public class Author
{
    public int ID { get; set; }
    public string Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfDeath { get; set; }

    public List<Novel> Novels { get; set; }
}
```

```csharp
public class Novel
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int AuthorId { get; set; }

    public Author Author { get; set; }

    public List<Nomination> Nominations { get; set; }
}
```

---

# Opening a Connection

```csharp
IDbConnection connection =
    new SqlConnection(connectionString);

connection.Open();
```

---

# Executing Queries

## Simple Query

```csharp
List<Novel> novels =
    connection.Query<Novel>(
        "SELECT * FROM Novels")
    .ToList();

connection.Close();
```

---

## Multiple Queries

```csharp
using (var multi = connection.QueryMultiple(
    "SELECT * FROM Novels; SELECT * FROM Nominations"))
{
    ...
}
```

`QueryMultiple()` allows multiple result sets to be returned from a single database call.

---

## Stored Procedures

```csharp
List<Author> authors =
    connection.Query<Author>(
        "dbo.getAuthors",
        new { authorName = nameFragment },
        commandType: CommandType.StoredProcedure)
    .ToList();
```

Using stored procedures is generally preferred over embedding SQL directly into the application because it improves maintainability and can provide additional security.

---

### 🤖 Let's Ask AI

```text
Explain how Dapper maps SQL results to C# objects.
```

```text
Explain QueryMultiple with a practical example.
```

```text
When should I use stored procedures with Dapper?
```

```text
Review this Dapper query and suggest improvements.
```

---

# Summary

In this lesson we learned:

- What ADO.NET is.
- What Dapper is.
- How Dapper differs from Entity Framework Core.
- How Dapper executes SQL queries.
- How Dapper maps results to C# objects.
- How to execute stored procedures.

---

# Extra Materials 📘

- Introduction to ADO.NET
- Microsoft ADO.NET Examples
- Dapper Documentation
- Using Dapper with C#
- CRUD with Dapper
- EF Core vs Dapper vs ADO.NET
- SQL Injection