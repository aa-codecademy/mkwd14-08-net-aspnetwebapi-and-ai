# Configuring Models in Entity Framework 🔸

Entity Framework Core allows us to configure how our C# models map to database tables.

Configuration determines:

- Table names
- Column names
- Relationships
- Validation rules
- Constraints
- Data types

There are two ways to configure models:

- Data Annotations
- Fluent API

---

# Data Annotations 🔸

Data Annotations are attributes placed directly above classes and properties.

They're simple to use and work well for smaller projects.

Common annotations include:

## Table

```csharp
[Table("Books")]
```

## Column

```csharp
[Column("ID")]
```

## Key

```csharp
[Key]
```

## ForeignKey

```csharp
[ForeignKey("Author")]
```

## Required

```csharp
[Required]
```

## MaxLength

```csharp
[MaxLength(150)]
```

## NotMapped

```csharp
[NotMapped]
```

---

### 🤖 Let's Ask AI

```text
Explain every commonly used Data Annotation.
```

```text
When should I use NotMapped?
```

```text
Explain Required vs nullable reference types.
```

---

# Fluent API 🔸

Fluent API provides a more powerful and flexible way of configuring models.

Instead of placing attributes inside entity classes, all configuration is centralized inside `OnModelCreating`.

This keeps domain models cleaner and makes complex configurations easier.

Examples include:

### HasColumnName

```csharp
entity.Property(e => e.Id)
      .HasColumnName("ID");
```

### IsRequired

```csharp
entity.Property(e => e.Name)
      .IsRequired();
```

### HasMaxLength

```csharp
entity.Property(e => e.Name)
      .HasMaxLength(100);
```

### Relationships

```csharp
entity.HasOne(d => d.Award)
      .WithMany(p => p.Nominations)
      .HasForeignKey(d => d.AwardId);
```

### HasColumnType

```csharp
entity.Property(e => e.DateOfBirth)
      .HasColumnType("date");
```

### Ignore

```csharp
entity.Ignore(e => e.NominationsCount);
```

---

### 🤖 Let's Ask AI

```text
Compare Fluent API and Data Annotations.
```

```text
Why do larger projects prefer Fluent API?
```

```text
Show me examples of relationships configured using Fluent API.
```

---

# Creating Complex Queries 🔸

Entity Framework Core allows us to query related data using navigation properties.

Simple query:

```csharp
IQueryable<Authors> result =
    _context.Set<Authors>();
```

Including one relationship:

```csharp
IQueryable<Authors> result =
    _context.Set<Authors>()
            .Include(x => x.Novels);
```

Including multiple levels:

```csharp
IQueryable<Authors> result =
    _context.Set<Authors>()
            .Include(x => x.Novels)
                .ThenInclude(x => x.Nominations)
                    .ThenInclude(x => x.Award);
```

Using `Include()` and `ThenInclude()` allows EF Core to eagerly load related entities and reduce the need for additional queries.

---

### 🤖 Let's Ask AI

```text
Explain Include and ThenInclude using simple examples.
```

```text
What is eager loading?
```

```text
What's the difference between eager loading, lazy loading and explicit loading?
```

```text
Review this EF Core query and suggest improvements.
```

---

# Summary

In this lesson we've learned:

- What Entity Framework Core is.
- How EF Core fits into a .NET Web API.
- Code First vs Database First.
- Scaffold-DbContext.
- Data Annotations.
- Fluent API.
- Building complex queries using Include and ThenInclude.

These concepts will serve as the foundation for working with relational databases throughout the rest of the course.

---

# Extra Materials 📘

- https://www.tektutorialshub.com/entity-framework-core/ef-core-console-application/
- https://www.learnentityframeworkcore.com/configuration/data-annotation-attributes
- https://www.learnentityframeworkcore.com/configuration/fluent-api