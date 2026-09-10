# ⭐ Bonus Homework — Lock the Movies API down 🔐

Your Movies API works. It also lets **anyone with the URL** do this:

```http
DELETE /api/movies/1
```

No token. No login. 204 No Content, and the movie is gone.

Fix that. Same solution, same layers — **do not start a new project.**

> **Prerequisite:** the JWT class. Everything below assumes you have seen `AddJwtBearer`,
> `[Authorize]` and BCrypt at least once.

---

## 1. The `User` entity

One new entity. **Fluent API, no Data Annotations** — the workshop rule did not expire.

| Field | Type | Notes |
|---|---|---|
| Id | int | |
| FirstName | string | required, max 50 |
| LastName | string | required, max 50 |
| Username | string | required, max 30, **unique** |
| PasswordHash | string | required, max 100 |
| Role | string / enum | `Admin` or `User` — your choice, be ready to defend it |

Configure the table as `User`, singular, plus a **unique index** on `Username`.

> ⚠ `USER` is a reserved T-SQL keyword. `SELECT * FROM User` fails in SSMS,
> `SELECT * FROM [User]` works. EF Core is unaffected.

One new migration, **`AddAuthentication`**, seeding **one `Admin`** and **one regular user**.

> ⚠ **The trap.** `BCrypt.HashPassword()` uses a random salt, so it returns a different string
> every time it runs. Call it inside `HasData` and *every* `Add-Migration` will generate a
> phantom migration — the same reason `HasData` refuses `DateTime.UtcNow`.
>
> Generate the hash **once**, paste it in as a literal, and write the plain password in a
> comment next to it. You cannot reverse a hash to get it back.

---

## 2. Two endpoints

| # | Method | Route | Success | Failure |
|---|---|---|---|---|
| 1 | POST | `/api/auth/register` | 201 + the created user (**no password, no hash**) | 400, **409** if the username is taken |
| 2 | POST | `/api/auth/login` | 200 + `{ "token": "..." }` | 400, **401** if the credentials are wrong |

Rules:

- **`register` always creates a `User`, never an `Admin`.** The role is not in the register DTO.
  An endpoint that lets anonymous callers make themselves an administrator is not an endpoint,
  it is a vulnerability.
- Password hashed with **BCrypt**, in the service layer. A plain password never reaches the database.
- The token carries three claims: **`ClaimTypes.NameIdentifier`** (id), **`ClaimTypes.Name`**
  (username) and **`ClaimTypes.Role`**.

> ⚠ **You can no longer query by username *and* password.** BCrypt salts every hash, so the stored
> string never equals the hash of what the client just typed. Load the user **by username only**,
> then `BCrypt.Verify(attempt, storedHash)`.
>
> ⚠ **"No such user" and "wrong password" get the same message and the same 401.** Tell an attacker
> which of the two it was and you have handed them a way to enumerate your usernames.

---

## 3. Now decide who gets in 🎯

Go through all endpoints from the workshop contract and decide,
for each one:

- Does it need a token at all?
- If it does — does it need a **specific role**?

---

## ⚠ One honest note about secrets

Your `SecretKey` is in `appsettings.json`, and `appsettings.json` is in git. Anyone who reads it
can forge a token saying `"role": "Admin"`, and your API will believe it — the signature will be
perfectly valid.

Fine for homework, wrong everywhere else. You do not have to fix it, but you do have to
**know it is broken** and be able to name the alternative (`dotnet user-secrets`, environment
variables, a key vault).

---

## 🤖 Let's Ask AI

Concepts, not code. Security code you cannot explain is worse than none, because it looks like
it is working.

```text
Explain the difference between 401 and 403, and why the names are historically confusing.
```

```text
Why does BCrypt produce a different hash every time, and how does Verify still work?
```

```text
Why can I no longer query the database by username AND password once passwords are hashed?
```

```text
What exactly goes wrong if UseAuthorization comes before UseAuthentication?
```

```text
Explain ClaimTypes.Role versus a custom "role" claim, and why [Authorize(Roles = "Admin")] cares.
```

```text
My HasData seed regenerates on every Add-Migration. Explain why a BCrypt hash causes that.
```

```text
Which endpoints in a public movie catalogue should require a login? Argue both sides.
```

### Avoid

```text
Add JWT authentication to my Movies API.
```

```text
Write my AuthService and AuthController.
```