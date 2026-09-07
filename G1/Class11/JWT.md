# JSON Web Tokens (JWT) 🔑

Now that we understand the difference between authentication and authorization, let's look at one of the most common authentication mechanisms used by modern Web APIs.

The most popular approach today is **JWT (JSON Web Token)**.

JWT enables clients and APIs to communicate securely without the server storing session information, making it ideal for stateless applications.

---

# What is JWT? 🔸

A **JSON Web Token (JWT)** is a compact, URL-safe token used to securely exchange information between a client and a server.

A JWT is:

- Digitally signed.
- Self-contained.
- Stateless.
- Easy to transmit in HTTP requests.

Instead of storing user information on the server, the required information is stored inside the token itself.

When the client sends the token with each request, the server verifies its authenticity before granting access.

---

# JWT Structure 🔸

A JWT consists of **three parts**, separated by dots (`.`):

```
Header.Payload.Signature
```

Each part has a specific purpose.

---

## Header

The Header contains metadata about the token.

Typical values include:

- Token type (`JWT`)
- Signing algorithm (`HS256`)

Example:

```json
{
    "alg": "HS256",
    "typ": "JWT"
}
```

---

## Payload

The Payload contains **claims**.

Claims are pieces of information about the authenticated user or the token itself.

Some common claims include:

| Claim | Description |
|--------|-------------|
| iss | Issuer |
| aud | Audience |
| sub | Subject |
| exp | Expiration time |
| iat | Issued at |
| role | User role |

Applications can also add custom claims, such as:

- UserId
- Username
- Email
- Department

---

## Signature

The Signature guarantees that the token hasn't been modified.

It is generated using:

- Header
- Payload
- Secret Key
- Signing Algorithm

If someone changes the Header or Payload, the generated signature will no longer match and the token becomes invalid.

![JWT Structure](https://cask.scotch.io/2014/11/json-web-token-overview1.png)

---

### 🤖 Let's Ask AI

```text
Explain every part of a JWT using simple language.
```

```text
What are claims inside a JWT?
```

```text
Why is the Signature important?
```

```text
Can someone modify the Payload of a JWT?
```

---

# Hashing 🔸

JWT relies on cryptographic algorithms to protect the integrity of the token.

A **hash function** converts input data of any size into a fixed-length value.

The resulting hash is deterministic:

- The same input always produces the same hash.
- Even a tiny change in the input produces a completely different hash.

Hashing is commonly used for:

- Passwords
- File verification
- Digital signatures
- Tokens

> **Note**
>
> The example below demonstrates the concept of hashing. Modern applications should **not** use **MD5** for password hashing. Instead, use secure algorithms such as **bcrypt**, **PBKDF2**, or **Argon2**.

Example:

```csharp
var md5 = new MD5CryptoServiceProvider();

var md5Data = md5.ComputeHash(
    Encoding.ASCII.GetBytes(password));

var hashedPassword =
    Encoding.ASCII.GetString(md5Data);
```

---

### 🤖 Let's Ask AI

```text
Explain hashing using a real-world analogy.
```

```text
What's the difference between hashing and encryption?
```

```text
Why shouldn't MD5 be used for password hashing anymore?
```

```text
Explain bcrypt, PBKDF2 and Argon2.
```

---

# How JWT Works 🔸

The authentication flow typically looks like this:

1. The client submits credentials.
2. The server validates the credentials.
3. A JWT is generated.
4. The token is returned to the client.
5. The client stores the token.
6. The client sends the token with every protected request.
7. The server validates the token.
8. If the token is valid, access is granted.

![JWT Authentication Flow](https://miro.medium.com/max/875/1*SSXUQJ1dWjiUrDoKaaiGLA.png)

---

### 🤖 Let's Ask AI

```text
Explain the JWT authentication flow step by step.
```

```text
Where should a frontend application store a JWT?
```

```text
What happens if someone changes a JWT?
```

```text
Why do JWTs expire?
```

---

# JWT Authentication in .NET 🔸

.NET provides built-in support for JWT authentication.

Authentication is configured during application startup.

The configuration includes:

- Registering JWT authentication.
- Configuring token validation.
- Defining the signing key.
- Registering the authentication middleware.

```csharp
builder.Services.AddAuthentication(...)
    .AddJwtBearer(...);

app.UseAuthentication();
app.UseAuthorization();
```

> **Important**
>
> Authentication middleware must always be registered **before** Authorization.

---

## Creating a JWT

After successfully authenticating a user, the server creates a JWT containing the required claims.

Typical claims include:

- User Id
- Name
- Username
- Roles

The token is digitally signed before being returned to the client.

The client then sends this token with every protected request.

---

### 🤖 Let's Ask AI

```text
Explain JWT configuration in .NET step by step.
```

```text
Why must UseAuthentication come before UseAuthorization?
```

```text
Explain what JwtBearer authentication does.
```

```text
Review this JWT configuration and explain every line.
```

---

# Authorization Attributes 🔸

.NET provides built-in authorization attributes for protecting API endpoints.

## AllowAnonymous

Allows requests without authentication.

```csharp
[AllowAnonymous]
[HttpPost("login")]
public IActionResult Login(LoginModel model)
{
    ...
}
```

---

## Authorize

Requires the user to be authenticated before accessing the endpoint.

```csharp
[Authorize]
[HttpGet]
public IActionResult Get()
{
    ...
}
```

You can apply `[Authorize]`:

- On individual actions.
- On entire controllers.

---

### 🤖 Let's Ask AI

```text
What's the difference between Authorize and AllowAnonymous?
```

```text
Can I protect an entire controller with Authorize?
```

```text
How does the Authorize attribute know whether a user is authenticated?
```

---

# Summary

In this lesson we learned:

- What a JWT is.
- The three parts of a JWT.
- What claims are.
- How hashing protects data.
- How JWT authentication works.
- How JWT authentication is configured in .NET.
- How to protect endpoints using `Authorize` and `AllowAnonymous`.

In the next lesson we'll look at **OAuth 2.0** and external authentication providers such as Google and Microsoft.