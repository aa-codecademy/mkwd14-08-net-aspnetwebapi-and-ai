# Homework - Class 03

## Objectives

In this homework, you'll build a simple **Books API** while practicing:

- Controllers
- Routing
- Query parameters
- Model Binding
- `FromBody`
- Testing endpoints with Postman and Swagger

---

## Requirements

1. Create a new **.NET Web API** project.

2. Create a new `BooksController`.

3. Create a `Book` model with the following properties:

```csharp
public class Book
{
    public string Author { get; set; }
    public string Title { get; set; }
}
```

4. Create a simple static database containing a list of `Book` objects.

5. Implement a **GET** endpoint that returns all books.

6. Implement a **GET** endpoint that returns a single book by its index using a **query parameter**.

Example:

```text
GET /api/books?index=2
```

7. Implement a **GET** endpoint that filters books by **author** and **title** using query parameters.

Example:

```text
GET /api/books/search?author=Robert Martin&title=Clean Code
```

8. Implement a **POST** endpoint that accepts a `Book` object from the request body using the **`[FromBody]`** attribute and adds it to the list.

9. Test all endpoints using both:

- Swagger
- Postman

---

## Bonus ⭐

Implement a **POST** endpoint that accepts a list of `Book` objects from the request body and returns only their titles as a `List<string>`.

Example request:

```json
[
  {
    "author": "Robert Martin",
    "title": "Clean Code"
  },
  {
    "author": "Martin Fowler",
    "title": "Refactoring"
  }
]
```

Example response:

```json
[
  "Clean Code",
  "Refactoring"
]
```

---

# 🤖 AI Guidelines

Use AI as a learning assistant—not as a code generator.

AI can help you:

- Understand model binding.
- Explain routing.
- Debug errors.
- Review your implementation.
- Suggest improvements after you've completed the homework.

### Good prompts

```text
Explain the difference between FromBody and FromQuery.
```

```text
Review my BooksController and suggest improvements without rewriting the solution.
```

```text
Why isn't my Book object being populated from the request body?
```

```text
Explain how model binding works in .NET Web API.
```

```text
Help me understand this compiler or runtime error without giving me the full solution.
```

```text
How should I test this endpoint in Postman?
```

### Avoid prompts like

```text
Generate the entire homework solution.
```

```text
Write the complete BooksController for me.
```

```text
Implement all endpoints.
```

The goal is to understand **how** the solution works, not simply generate working code.