# Movie API 🎬

In this workshop, we'll build our first complete **Movie API** by applying what we've learned so far.

The goal is to practice:

- Controllers
- Routing
- Query Parameters
- Route Parameters
- Model Binding
- CRUD operations
- DTOs
- Validation

We'll use a simple **static database**, allowing us to focus on API development without introducing a real database yet.

---

# Requirements

Create a **.NET Web API** that manages a collection of movies.

The API should support the following operations:

## Get Movie by Id

Implement two endpoints:

- Using a **route parameter**
- Using a **query parameter**

Examples:

```text
GET /api/movies/5
```

```text
GET /api/movies?id=5
```

---

## Get All Movies

Create an endpoint that returns all movies.

Example:

```text
GET /api/movies
```

---

## Filter Movies

Create an endpoint that filters movies by:

- Genre
- Year

The endpoint should allow filtering by:

- Genre only
- Year only
- Both Genre and Year

Example:

```text
GET /api/movies/filter?genre=Comedy
```

```text
GET /api/movies/filter?year=2024
```

```text
GET /api/movies/filter?genre=Comedy&year=2024
```

---

## Create Movie

Create an endpoint that adds a new movie.

Use **POST** together with **`[FromBody]`**.

---

## Update Movie

Create an endpoint that updates an existing movie.

Choose an appropriate HTTP method and explain why you selected it.

---

## Delete Movie

Implement two delete endpoints.

### Delete using Route Parameter

Example:

```text
DELETE /api/movies/5
```

### Delete using Request Body

Accept the movie id from the request body.

---

# Movie Model

A movie contains:

- Id
- Title *(required)*
- Description *(optional)*
- Year *(required)*
- Genre *(required)*

---

# DTOs

Use **DTOs** for transferring data between the client and the API.

Avoid exposing internal entities directly.

---

# Validation

Implement the following validation rules:

- Title is required.
- Year is required.
- Genre is required.
- Description is optional.
- If Description is provided, its maximum length is **250 characters**.

---

# Suggested API Endpoints

| Method | Endpoint |
|---------|----------|
| GET | `/api/movies` |
| GET | `/api/movies/{id}` |
| GET | `/api/movies?id=1` |
| GET | `/api/movies/filter` |
| POST | `/api/movies` |
| PUT | `/api/movies/{id}` |
| DELETE | `/api/movies/{id}` |
| DELETE | `/api/movies` |

---

# 🤖 Let's Ask AI

Use AI to better understand the concepts—not to generate the complete solution.

### Good prompts

```text
Help me design a RESTful Movie API.
```

```text
Review my controller and suggest improvements without rewriting the solution.
```

```text
What's the best HTTP method for updating a movie and why?
```

```text
Explain how to combine multiple query parameters in one endpoint.
```

```text
Review my validation rules and suggest improvements.
```

```text
Explain when route parameters are better than query parameters.
```

```text
Review my DTOs and explain whether they follow .NET best practices.
```

---

### Avoid prompts like

```text
Generate the entire Movie API.
```

```text
Write the whole controller for me.
```

```text
Implement the complete workshop.
```

The goal is to understand **how to build the API**, not simply generate working code.