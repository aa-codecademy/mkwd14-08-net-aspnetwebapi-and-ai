# Database Access with ADO.NET 🗄️

In the previous lesson, we learned how to work with databases using Entity Framework Core.

Although EF Core is one of the most popular ORMs in .NET, it isn't the only way to communicate with a database.

In this lesson, we'll explore two additional approaches:

- ADO.NET
- Dapper

Understanding these technologies will help you choose the right tool depending on your application's requirements.

---

# Looking Back... 🔙

Before we continue, let's quickly review:

- What is Entity Framework Core?
- What is an ORM?
- What is Code First?
- What is Database First (Scaffolding)?
- What are Data Annotations?
- What is Fluent API?

---

# Other Database Frameworks 🔸

Entity Framework Core is not the only way to communicate with a relational database.

There are many libraries and frameworks that simplify working with databases.

Some of the most common approaches are:

- Entity Framework Core
- Dapper
- ADO.NET

Each approach offers different advantages depending on the project requirements.

---

# ADO.NET 🔸

**ADO.NET (Active Data Objects .NET)** is Microsoft's low-level data access technology.

Unlike Entity Framework Core, ADO.NET does not perform any automatic mapping between database tables and C# objects.

Instead, developers are responsible for:

- Opening database connections
- Executing SQL queries
- Reading data
- Mapping data manually
- Closing database connections

Because of this, ADO.NET offers excellent performance and full control over database communication.

---

## How ADO.NET Works

A typical ADO.NET workflow consists of the following steps:

1. Configure the connection string.
2. Create a database connection.
3. Open the connection.
4. Create an SQL command.
5. Execute the command.
6. Read the returned data.
7. Map the data to objects.
8. Close the connection.

Although this requires more code than Entity Framework Core, it also provides more control.

---

### 🤖 Let's Ask AI

```text
Explain the difference between ADO.NET and Entity Framework Core.
```

```text
Why is ADO.NET considered a low-level data access technology?
```

```text
What are the advantages and disadvantages of ADO.NET?
```

```text
When would you choose ADO.NET over Entity Framework Core?
```

---

# Summary

ADO.NET gives developers complete control over database communication.

However, because everything must be done manually, applications usually require more code compared to modern ORMs such as Entity Framework Core or Dapper.