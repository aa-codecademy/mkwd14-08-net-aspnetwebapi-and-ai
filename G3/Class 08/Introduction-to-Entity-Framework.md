# Building a .NET Web API with Entity Framework 🥐

As our applications grow, storing data in static classes is no longer practical.

In this lesson, we'll introduce **Entity Framework Core (EF Core)**, Microsoft's Object-Relational Mapper (ORM), and learn how it simplifies working with relational databases.

We'll also explore how EF Core fits into a .NET Web API application and how it helps us build clean, maintainable and scalable applications.

---

# Looking Back... 🔙

Before we continue, let's quickly review:

- What are query parameters?
- How can we pass models to an API?
- What is Postman?
- What is Swagger?
- What's the difference between Postman and Swagger?

---

# Building .NET Web API Applications 🔸

Building a Web API follows the same software engineering principles as building any modern application.

As projects grow, having a good architecture becomes increasingly important.

Some of the key concepts we'll rely on throughout the course are:

- Separation of concerns
- Layered architecture
- Design patterns
- Dependency Injection
- Object-Relational Mapping (ORM)

Libraries such as **Entity Framework Core** help abstract complex database operations so we can work with strongly typed C# objects instead of writing SQL for every operation.

---

### 🤖 Let's Ask AI

```text
Explain why software architecture becomes more important as applications grow.
```

```text
What is an ORM and why do modern applications use one?
```

```text
Compare writing raw SQL with using Entity Framework Core.
```

---

# What is Entity Framework Core? 🔸

Entity Framework Core (EF Core) is Microsoft's Object-Relational Mapper (ORM).

It bridges the gap between a relational database and C# code by mapping database tables to classes and database records to objects.

Instead of manually writing SQL for every operation, EF Core allows us to work with objects while generating the required SQL behind the scenes.

Some of the things EF Core helps us with include:

- CRUD operations
- Relationships
- Validation
- Database configuration
- Migrations
- Query generation
- Change tracking

---

# Entity Framework in .NET Web APIs

Entity Framework Core integrates naturally with .NET Web APIs.

It allows our API to:

- Connect to SQL Server.
- Read data.
- Insert data.
- Update existing records.
- Delete records.
- Build complex database queries.

Instead of manually opening SQL connections and mapping data, EF Core handles these tasks automatically.

---

### 🤖 Let's Ask AI

```text
Explain how Entity Framework Core works internally.
```

```text
Why is EF Core called an Object-Relational Mapper?
```

```text
What are the advantages and disadvantages of EF Core?
```

---

# Entity Framework in Console Applications 🔸

Unlike .NET Web API projects, Console Applications don't include Entity Framework Core by default.

To use EF Core inside a Console Application, we need to install the required NuGet packages.

The most common packages are:

- **Microsoft.EntityFrameworkCore.SqlServer**
- **Microsoft.EntityFrameworkCore.Design**
- **Microsoft.EntityFrameworkCore.Tools**

Each package has a different purpose:

| Package | Purpose |
|---------|----------|
| Microsoft.EntityFrameworkCore.SqlServer | SQL Server provider |
| Microsoft.EntityFrameworkCore.Design | Enables migrations |
| Microsoft.EntityFrameworkCore.Tools | Adds EF Core CLI and Package Manager commands |

---

### 🤖 Let's Ask AI

```text
Explain the purpose of each Entity Framework Core NuGet package.
```

```text
Why do we need different EF Core packages?
```

```text
What's the difference between Design and Tools packages?
```

---

# Scaffolding a DbContext 🔸

Sometimes we don't start with an empty database.

Many real-world projects already have an existing SQL Server database.

Instead of manually creating all domain models, EF Core allows us to generate them automatically using **Scaffold-DbContext**.

Scaffolding creates:

- DbContext
- Entity classes
- Relationships

directly from an existing database.

---

## Scaffold-DbContext

The command requires:

- Connection String
- Database Provider
- Output Directory

Example:

```powershell
Scaffold-DbContext "Server=.\SQLExpress;Database=BooksDB2022;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Domain
```

After executing the command, EF Core generates the DbContext together with all entity classes based on the database schema.

---

### 🤖 Let's Ask AI

```text
Explain Code First vs Database First.
```

```text
When should I use Scaffold-DbContext?
```

```text
Explain every part of the Scaffold-DbContext command.
```