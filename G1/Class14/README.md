# Logging 🚁

## Looking Back

In the previous lessons, we learned how to:

- Secure APIs using Authentication and Authorization
- Work with JWT tokens
- Understand OAuth 2.0 and external authentication providers
- Write automated tests

Building a good API is only part of the job. In real-world applications, we also need to monitor what happens inside our applications.

In this lesson, we'll learn about:

- What logging is and why it matters
- Log levels
- Logging libraries in .NET
- Serilog: installing, configuring and writing logs

---

# Logging 🔶

Logging is the process of recording information about what happens inside an application.

Logs help developers:

- Detect and investigate errors
- Troubleshoot production issues
- Monitor application behavior
- Understand what happened without using a debugger

When an application is running on a development machine, we can often use Visual Studio to debug issues. However, once the application is deployed to a test or production server, debugging is usually no longer possible. In those situations, log files become one of the most valuable tools for identifying and understanding problems.

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

Using log levels makes it easier to filter logs and quickly identify important events.

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

Each library offers similar functionality, with differences in configuration, integrations, and supported features.

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

In addition to automatic logging, developers can easily create their own custom log messages using the `Log` class.

---

## Installing Serilog

To use Serilog in a .NET application, install the following NuGet packages:

- `Serilog`
- `Serilog.AspNetCore`
- `Serilog.Sinks.File`

These packages provide the core logging functionality, ASP.NET Core integration, and the ability to write logs to a file.

---

## Configuring Serilog

The following example shows a simple Serilog configuration in `Program.cs`.

```csharp
// In Program.cs
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.File("logs.txt"));
```

This configuration writes application logs to a file named `logs.txt`.

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

Notice the use of placeholders (`{username}`, `{userId}`) and structured logging (`{@user}`), which allows entire objects to be recorded in a structured format instead of converting them into plain text.

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

# Summary

In this lesson we learned:

- Why logging is important.
- The most common logging levels.
- Popular .NET logging libraries.
- How Serilog is configured and used.
- How to write plain and structured log messages.

---

# Extra Materials 📘

- https://github.com/serilog/serilog/wiki/Writing-Log-Events
- https://github.com/serilog/serilog/wiki/Configuration-Basics
- https://learn.microsoft.com/aspnet/core/fundamentals/logging/
- https://michaelscodingspot.com/logging-in-dotnet/
