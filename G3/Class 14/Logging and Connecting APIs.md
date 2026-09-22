# Logging and Connecting APIs 🚁

## Looking Back

In the previous lessons, we learned how to:

- Secure APIs using Authentication and Authorization
- Work with JWT tokens
- Understand OAuth 2.0 and external authentication providers
- Write automated tests

Building a good API is only part of the job. In real-world applications, we also need to monitor what happens inside our applications and communicate with other services.

In this lesson, we'll learn about:

- Logging
- Connecting applications using HTTP
- Microservice communication

---

# Logging 🔶

Logging is the process of recording information about what happens inside an application.

Logs help developers:

- Detect and investigate errors
- Troubleshoot production issues
- Monitor application behavior
- Understand what happened without using a debugger

When an application is running on a development machine, we can often use Visual Studio to debug issues. However, once the application is deployed to a test or production server, debugging is usually no longer possible. In those situations, log files become one of the most valuable tools for identifying and understanding problems. :contentReference[oaicite:0]{index=0}

---

### 🤖 Let's Ask AI

```text
Explain logging in software development.
```

```text
Why is logging important in production?
```

```text
What's the difference between debugging and logging?
```

```text
Give examples of situations where logging is useful.
```

---

# Log Messages 🔶

Logging consists of writing messages that describe events occurring inside an application.

Log messages can be written to:

- Text files
- Databases
- Cloud logging platforms
- Monitoring systems

Not every message has the same importance. Most logging libraries classify log messages using different **log levels**.

The most common log levels are:

| Level | Description |
|--------|-------------|
| Error | An error or exception occurred. |
| Warning | Something unexpected happened, but the application can continue running. |
| Information | General information about application behavior. |
| Debug | Detailed information useful during development and debugging. |

Using log levels makes it easier to filter logs and quickly identify important events. :contentReference[oaicite:1]{index=1}

---

### 🤖 Let's Ask AI

```text
Explain the different logging levels.
```

```text
When should I use Warning instead of Error?
```

```text
Give examples of Information and Debug logs.
```

---

# Logging Libraries 🔶

Although it's possible to create a custom logger that writes messages to a file, most .NET applications use dedicated logging libraries.

Logging libraries provide many useful features out of the box, including:

- Automatic timestamps
- Multiple log levels
- Structured logging
- Logging to different destinations (File, Console, Database, Cloud)
- Better filtering and searching

Some of the most popular logging libraries for .NET applications are:

- Serilog
- NLog
- Log4Net

Each library offers similar functionality, with differences in configuration, integrations, and supported features. :contentReference[oaicite:2]{index=2}

---

### 🤖 Let's Ask AI

```text
Compare Serilog, NLog, and Log4Net.
```

```text
Why do .NET projects use logging libraries?
```

```text
What is structured logging?
```

---

# Serilog 🔶

**Serilog** is one of the most widely used logging libraries in the .NET ecosystem.

After configuration, Serilog can automatically record useful information such as:

- Application startup
- Incoming HTTP requests
- Executed endpoints
- SQL queries
- Timestamps
- Exceptions

In addition to automatic logging, developers can easily create their own custom log messages using the `Log` class. :contentReference[oaicite:3]{index=3}

---

## Installing Serilog

To use Serilog in a .NET application, install the following NuGet packages:

- `Serilog`
- `Serilog.AspNetCore`
- `Serilog.Sinks.File`

These packages provide the core logging functionality, ASP.NET Core integration, and the ability to write logs to a file. :contentReference[oaicite:4]{index=4}

---

## Configuring Serilog

The following example shows a simple Serilog configuration in `Program.cs`.

```csharp
// In Program.cs
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.File("logs.txt"));
```

This configuration writes application logs to a file named `logs.txt`. :contentReference[oaicite:5]{index=5}

---

## Writing Log Messages

Once configured, logging becomes very simple.

```csharp
// Information log
Log.Information("USER {username} has registered on the map", Username);

// Error log
Log.Error("USER Error for {userId}.{name}: {message}", UserId, Name, Message);

// Structured logging
Log.Error("USER Error for {@user}: {message}", User, Message);
```

Notice the use of placeholders (`{username}`, `{userId}`) and structured logging (`{@user}`), which allows entire objects to be recorded in a structured format instead of converting them into plain text. :contentReference[oaicite:6]{index=6}

---

### 🤖 Let's Ask AI

```text
Explain structured logging in Serilog.
```

```text
Generate examples of Information, Warning, and Error logs.
```

```text
Why is structured logging better than string concatenation?
```

---

# Connecting Applications 🔶

Modern applications rarely work alone.

A typical application communicates with many other systems, including:

- Other APIs
- Mobile applications
- Web applications
- Console applications
- Third-party services

Most of this communication happens using the HTTP protocol. :contentReference[oaicite:7]{index=7}

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

For example, if authentication is handled by a dedicated Authentication Service, the rest of the application simply communicates with it instead of implementing authentication logic itself. :contentReference[oaicite:8]{index=8}

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

# HttpClient 🔶

Applications communicate with each other using the **HTTP protocol**.

In .NET, the most common way to send HTTP requests is by using the **HttpClient** class.

With `HttpClient`, an application can:

- Send GET requests
- Send POST requests
- Send PUT requests
- Send DELETE requests
- Read HTTP responses

This allows .NET applications to communicate with Web APIs, third-party services, and other applications. :contentReference[oaicite:0]{index=0}

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

:contentReference[oaicite:1]{index=1}

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

Typically, the addresses of external APIs are stored in configuration files such as `appsettings.json`, making them easier to manage across different environments. :contentReference[oaicite:2]{index=2}

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

:contentReference[oaicite:3]{index=3}

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

This ensures smooth communication between the two applications. :contentReference[oaicite:4]{index=4}

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

# Summary

In this lesson we learned:

- Why logging is important.
- The most common logging levels.
- Popular .NET logging libraries.
- How Serilog is configured and used.
- How applications communicate over HTTP.
- The basics of microservice architecture.
- How to use `HttpClient`.
- How APIs communicate with other applications.

---

# Extra Materials 📘

- https://github.com/serilog/serilog/wiki/Writing-Log-Events
- https://github.com/serilog/serilog/wiki/Configuration-Basics
- https://learn.microsoft.com/aspnet/core/fundamentals/http-requests
- https://learn.microsoft.com/dotnet/core/extensions/httpclient-factory
- https://michaelscodingspot.com/logging-in-dotnet/