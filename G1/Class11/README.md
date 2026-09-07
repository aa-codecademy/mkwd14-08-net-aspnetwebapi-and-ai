# Authentication and Authorization 🔐

Modern Web APIs are often accessed by multiple clients, including web applications, mobile apps, desktop applications and third-party services.

Unlike traditional MVC applications, APIs cannot rely on browser sessions or HTML pages to secure communication. Instead, they use authentication and authorization mechanisms to verify users and protect resources.

In this lesson, we'll explore the fundamentals of API security and understand how clients securely communicate with APIs.

---

# Looking Back... 🔙

Before we continue, let's quickly review:

- What are some alternatives to Entity Framework Core?
- How does ADO.NET work?
- Why is ADO.NET so fast?
- What are the differences between Dapper and Entity Framework Core?

---

# Securing Web APIs 🔸

Every Web API should ensure that only authorized users and applications can access protected resources.

A secure API should answer two important questions:

- **Who is making this request?**
- **Is this user allowed to perform this action?**

These questions are answered through **Authentication** and **Authorization**.

---

# Authentication vs Authorization 🔸

Authentication and Authorization are closely related, but they solve different problems.

## Authentication

Authentication is the process of verifying the identity of a user or application.

It answers the question:

> **Who are you?**

Examples:

- Logging in with a username and password.
- Signing in with Google.
- Signing in with Microsoft.
- Providing an API key.

Without successful authentication, the application doesn't know who is making the request.

---

## Authorization

Authorization determines what an authenticated user is allowed to do.

It answers the question:

> **What are you allowed to access?**

Examples:

- Reading notes.
- Creating new notes.
- Deleting users.
- Accessing administrator endpoints.

Authorization always happens **after** successful authentication.

---

### 🤖 Let's Ask AI

```text
Explain the difference between authentication and authorization using real-world examples.
```

```text
Give me examples where authentication succeeds but authorization fails.
```

```text
Why must authentication happen before authorization?
```

---

# Securing API Communication 🔸

Unlike MVC applications, Web APIs don't know whether the client is:

- A browser
- A mobile application
- Another API
- A desktop application
- An IoT device

Because of this, every request must contain enough information for the API to verify the caller.

The two most common approaches are:

- Session-based Authentication
- Token-based Authentication

---

# Session-Based Authentication 🔸

Session-based authentication is also known as **stateful authentication**.

After a successful login:

1. The user submits their credentials.
2. The server validates them.
3. The server creates a session.
4. A session cookie is returned to the client.
5. The client sends the cookie with every request.
6. The server validates the session before processing the request.

Because the server stores session information, this approach is called **stateful**.

---

## Advantages

- Simple to understand.
- Easy to implement.
- Common for traditional web applications.

## Disadvantages

- The server must store session data.
- Scaling across multiple servers is more difficult.
- Less suitable for distributed systems and public APIs.

---

# Token-Based Authentication 🔸

Token-based authentication is also known as **stateless authentication**.

Instead of storing sessions on the server:

1. The user logs in.
2. The server validates the credentials.
3. A token is generated.
4. The client stores the token.
5. The client sends the token with every request.
6. The API validates the token before processing the request.

Because the server doesn't store client state, this approach is called **stateless**.

![Stateful vs Stateless Authentication](https://cdn.auth0.com/blog/cookies-vs-tokens/cookie-token-auth.png)

---

## Advantages

- Highly scalable.
- Well suited for APIs.
- Works well across multiple servers.
- Easy to consume from mobile and frontend applications.

## Disadvantages

- Tokens must be protected carefully.
- Revoking tokens can be more difficult.
- Expiration and refresh strategies need to be considered.

---

### 🤖 Let's Ask AI

```text
Explain the difference between stateful and stateless authentication.
```

```text
Why do modern Web APIs prefer token-based authentication?
```

```text
Compare session-based authentication and token-based authentication.
```

```text
In what scenarios would session-based authentication still be a good choice?
```

---

# Summary

In this lesson we learned:

- Why Web APIs require authentication.
- The difference between Authentication and Authorization.
- Stateful vs Stateless authentication.
- Session-based authentication.
- Token-based authentication.

In the next lesson we'll take a closer look at **JSON Web Tokens (JWT)** and see how they're used to secure modern .NET Web APIs.