# Postman and Swagger 🧪

# Postman 🔸

Postman is an application used for sending and receiving HTTP requests easily.

It is one of the most popular tools for testing APIs because it allows developers to create custom requests without needing a frontend application.

Postman can be used to:

- Send requests using different HTTP methods
- Add headers
- Add query parameters
- Send request bodies
- Save requests
- Organize requests into collections
- Share requests with teammates
- Write request tests

---

# Installing Postman 🔽

Postman can be downloaded from:

```text
https://www.postman.com/downloads/
```

The installation is simple and the application can be used immediately.

You can use Postman without logging in, but creating an account allows you to sync collections and collaborate more easily.

---

# Sending Requests in Postman 🔽

To send a request in Postman:

1. Open Postman.
2. Choose the HTTP method.
3. Enter the request URL.
4. Add query parameters if needed.
5. Add headers if needed.
6. Add body data if needed.
7. Send the request.
8. Inspect the response.

Postman contains several important tabs:

- **Params** - used for query parameters.
- **Authorization** - used for request authentication.
- **Headers** - used for request headers.
- **Body** - used for sending data.
- **Pre-request Scripts** - scripts executed before sending the request.
- **Tests** - JavaScript tests executed after receiving the response.

### 🤖 Let's Ask AI

```text
Explain how to test a GET endpoint in Postman.
```

```text
Explain how to send a JSON body in Postman.
```

```text
What is the difference between Params, Headers and Body in Postman?
```

---

# Postman Collections 🔽

A collection is a place where we store related API requests.

Collections help us organize requests and reuse them later.

They are useful when:

- Testing the same API multiple times
- Sharing requests with teammates
- Organizing endpoints by feature
- Keeping examples for homework or demos

Example collection structure:

```text
Notes API
  GET all notes
  GET note by id
  POST create note
  PUT update note
  DELETE note
```

### 🤖 Let's Ask AI

```text
Suggest a good Postman collection structure for a Notes API.
```

```text
Why are Postman collections useful when testing APIs?
```

```text
Create a checklist for testing CRUD endpoints in Postman.
```

---

# Swagger 🔸

Swagger is a library used for mapping API endpoints and creating an interface for testing them.

Unlike Postman, which is a separate application, Swagger is installed and configured inside the API project.

Swagger automatically detects:

- Controllers
- Actions
- Routes
- HTTP methods
- Request models
- Response models

It generates an interactive UI where developers can test endpoints directly from the browser.

Swagger is very helpful for:

- Testing endpoints
- Exploring available API routes
- Understanding request and response models
- Sharing API documentation

---

# Swagger Configuration 🔽

Swagger can be added through the `Swashbuckle.AspNetCore` NuGet package.

In modern .NET Web API projects, Swagger configuration is usually added in `Program.cs`.

```csharp
builder.Services.AddSwaggerGen();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

After running the application, Swagger UI can usually be accessed through:

```text
/swagger/index.html
```

### 🤖 Let's Ask AI

```text
Explain what Swagger is and why it is useful.
```

```text
What is the difference between Swagger and Postman?
```

```text
Explain this Swagger configuration line by line.
```

---

# How to Use Swagger 🔽

When the application is running, Swagger displays all available endpoints.

From Swagger UI we can:

- View available controllers.
- View available actions.
- Send test requests.
- Enter route parameters.
- Enter query parameters.
- Send request body data.
- Inspect responses.

Swagger is especially useful while developing an API because it gives us immediate feedback about our endpoints.

### 🤖 Let's Ask AI

```text
How do I test a POST endpoint using Swagger?
```

```text
Why doesn't my endpoint appear in Swagger?
```

```text
How can Swagger help frontend developers understand my API?
```

---

# Extra Materials 📘

- http://www.cheat-sheets.org/sites/html.su/urlencoding.html
- https://www.freeformatter.com/url-parser-query-string-splitter.html
- https://medium.com/aubergine-solutions/api-testing-using-postman-323670c89f6d
- http://hamidmosalla.com/2017/07/06/asp-net-core-model-binding-controlling-the-binding-source/
- https://dev.to/lucas0707/how-to-quickly-install-swagger-in-a-net-core-application-jkc