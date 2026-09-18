# Connecting APIs 🚁

## Looking Back

In the previous lesson, we learned how to:

- Record what happens inside an application using logging
- Use log levels to separate important events from noise
- Configure Serilog and write structured log messages

Applications rarely work alone. In this lesson, we'll learn how our API talks to other applications:

- Connecting applications over HTTP
- Microservice communication
- `HttpClient`
- Calling another API, from a console app and from a front-end
- CORS: why the browser blocks your front-end, and how to allow it

---

# Connecting Applications 🔶

Modern applications rarely work alone.

A typical application communicates with many other systems, including:

- Other APIs
- Mobile applications
- Web applications
- Console applications
- Third-party services

Most of this communication happens using the HTTP protocol.

---

### 🤖 Let's Ask AI

```text
Why do modern applications communicate with other services?
```

```text
Explain API-to-API communication.
```

---

# Microservice Architecture 🔶

One popular way of building modern applications is using a **microservice architecture**.

Instead of placing all business logic into a single application, the system is divided into multiple smaller services.

Each service has a single responsibility.

For example:

- User Service
- Order Service
- Payment Service
- Notification Service

These services communicate with one another to create the complete application.

This approach offers several advantages:

- Better separation of responsibilities
- Easier maintenance
- Independent deployment
- Improved scalability
- Easier replacement of individual services

For example, if authentication is handled by a dedicated Authentication Service, the rest of the application simply communicates with it instead of implementing authentication logic itself.

---

### 🤖 Let's Ask AI

```text
Explain microservices using a real-world example.
```

```text
Compare monolithic and microservice architectures.
```

```text
What are the advantages of microservices?
```

---

# HttpClient 🔶

Applications communicate with each other using the **HTTP protocol**.

In .NET, the most common way to send HTTP requests is by using the **HttpClient** class.

With `HttpClient`, an application can:

- Send GET requests
- Send POST requests
- Send PUT requests
- Send DELETE requests
- Read HTTP responses

This allows .NET applications to communicate with Web APIs, third-party services, and other applications.

---

## Creating and Using an HttpClient

The following example demonstrates how to create an `HttpClient`, send a GET request, and read the response.

```csharp
// Creating the HTTP Client
HttpClient client = new HttpClient();

// Setting URL
string url = "www.myapi.com/api/users";

// Making the call and getting response
HttpResponseMessage response = client.GetAsync(url).Result;

// Getting the response body from the response
string responseBody = response.Content.ReadAsStringAsync().Result;
```

> **Note**
>
> In modern .NET applications, `HttpClient` is usually registered through **Dependency Injection** using `IHttpClientFactory` instead of creating it directly with `new HttpClient()`. The example above demonstrates the basic concept of making HTTP requests.

---

### 🤖 Let's Ask AI

```text
Explain how HttpClient works.
```

```text
Generate a simple GET request using HttpClient.
```

```text
Why is IHttpClientFactory recommended in modern .NET applications?
```

```text
What's the difference between GET and POST requests when using HttpClient?
```

---

# Calling Another API 🔶

Web APIs don't only communicate with client applications—they can also communicate with other APIs.

In a microservice architecture, it is common for one API to call another API to retrieve or send data.

For example:

- An **Order API** might call a **User API** to retrieve customer information.
- A **Payment API** might call a **Notification API** after a successful payment.
- An **Authentication API** might validate user credentials for multiple applications.

Typically, the addresses of external APIs are stored in configuration files such as `appsettings.json`, making them easier to manage across different environments.

---

### 🤖 Let's Ask AI

```text
Explain API-to-API communication.
```

```text
Why are API URLs usually stored in appsettings.json?
```

```text
Give examples of APIs communicating with each other.
```

---

# Console Applications 🔶

Console applications can also communicate with Web APIs.

Using `HttpClient`, a console application can:

- Send HTTP requests
- Retrieve data
- Display information
- Perform administrative tasks

This is useful for:

- Testing APIs
- Importing or exporting data
- Running scheduled jobs
- Building internal tools

---

### 🤖 Let's Ask AI

```text
Show how a console application can call a Web API.
```

```text
What are common use cases for console applications that consume APIs?
```

---

# Front-End Applications 🔶

One of the most common API consumers is a **front-end application**.

Front-end frameworks such as:

- Angular
- React
- Vue

communicate with backend APIs by sending HTTP requests.

The backend API processes the request and returns data, usually in **JSON** format.

The front-end then uses that data to display information or update the user interface dynamically.

When building APIs, it's important that both the frontend and backend agree on:

- Request formats
- Response formats
- Endpoints
- HTTP methods

This ensures smooth communication between the two applications.

---

### 🤖 Let's Ask AI

```text
Explain how a frontend communicates with a backend API.
```

```text
What format do Web APIs usually return?
```

```text
Why is JSON commonly used in Web APIs?
```

---

# CORS 🔶

If you open the front-end you just built and call your API, there's a good chance nothing happens — and the browser console shows something like this:

```text
Access to fetch at 'https://localhost:7118/api/notes' from origin 'http://localhost:5500'
has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present
on the requested resource.
```

The confusing part is that **the API is fine**. Look at the server logs and you'll see the request arrived and returned `200 OK`. Nothing is broken — the browser simply refused to hand the response to your JavaScript.

---

## Why This Happens

Browsers enforce a rule called the **same-origin policy**. JavaScript running on one origin is not allowed to read responses from a different origin.

An **origin** is the combination of three things:

- Scheme (`http` vs `https`)
- Host (`localhost`, `myapp.com`)
- Port (`5500`, `7118`)

If any one of them differs, it's a different origin. So `http://localhost:5500` calling `https://localhost:7118` is a cross-origin request — same machine, still blocked.

This rule exists for your protection. Without it, any website you visited could quietly send requests to your bank's API using your logged-in session and read the response.

**CORS** (Cross-Origin Resource Sharing) is how an API says: *"it's fine, I trust that origin."* The API adds response headers, and the browser then allows it.

> **Important**
>
> CORS is a **browser** rule, not an HTTP rule.
>
> The console application and the API-to-API calls from the previous sections use `HttpClient` — they are not browsers, so CORS never applies to them. The same request that a browser blocks works perfectly from Postman, from a console app, or from another API. This is why "it works in Postman but not in my JavaScript" is such a common confusion.

---

## Enabling CORS in ASP.NET Core

CORS is enabled in two steps, both in `Program.cs`.

**1. Register a policy** (before `builder.Build()`):

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

**2. Use the policy** in the middleware pipeline:

```csharp
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();
```

That's all it takes. Refresh the front-end and the request goes through.

---

## Order Matters ⚠

`app.UseCors()` must come **before** `app.UseAuthorization()` and `app.MapControllers()`.

Middleware runs in the order you register it. If `UseCors` comes after `MapControllers`, the controller has already produced the response and the CORS headers are never added — so the browser blocks it and you see the exact same error, even though the code "looks right". This is easily the most common CORS mistake.

---

## A Realistic Policy

`AllowAll` is a classroom shortcut. It tells the browser that *any* website on the internet may call your API.

In a real application you list the origins you actually trust:

```csharp
options.AddPolicy("FrontEnd", policy =>
{
    policy.WithOrigins("http://localhost:5500", "https://myapp.com")
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials();
});
```

> **Note**
>
> `AllowAnyOrigin()` and `AllowCredentials()` cannot be combined. If you try, the app throws at startup:
>
> ```text
> System.InvalidOperationException: The CORS protocol does not allow specifying
> a wildcard (any) origin and credentials at the same time.
> ```
>
> Once you need credentials — cookies, or an `Authorization` header carrying your JWT — you must name the origins explicitly with `WithOrigins(...)`.

---

### 🤖 Let's Ask AI

```text
Explain the same-origin policy like I'm new to web development.
```

```text
Why does my API call work in Postman but fail in the browser?
```

```text
What is a CORS preflight request and when does the browser send one?
```

```text
Why is AllowAnyOrigin() a bad idea in production?
```

---

# Summary

In this lesson we learned:

- How applications communicate over HTTP.
- The basics of microservice architecture.
- How to use `HttpClient`.
- How APIs communicate with other APIs.
- How console and front-end applications consume a Web API.
- Why browsers block cross-origin requests, and how to enable CORS in ASP.NET Core.

---

# Extra Materials 📘

- https://learn.microsoft.com/aspnet/core/fundamentals/http-requests
- https://learn.microsoft.com/dotnet/core/extensions/httpclient-factory
- https://learn.microsoft.com/dotnet/api/system.net.http.httpclient
- https://learn.microsoft.com/aspnet/core/security/cors
- https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS
