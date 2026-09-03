# Homework - Class 09

## 🐛 The Bug Hunt

This homework is different. **You are not writing a new API — you are fixing one.**

In `code/LibraryApi/` there is a small, finished **Library API**: a Books controller,
a service layer, a repository layer and a SQL Server database. It builds. It runs. Swagger opens.
Nothing crashes.

It is also **wrong**. Our imaginary QA tester played with it and filed **4 bug reports**
(plus one they weren't sure about). Every bug is a *logic* bug — the compiler is happy, the code
just does the wrong thing.

Your job: reproduce each report, find the cause, fix it, and explain it.

---

## Objectives

In this homework you'll practice:

- Reading and navigating an N-tier solution you did not write
- Reproducing a bug before trying to fix it
- Narrowing a bug down layer by layer — controller → service → mapper → repository → database
- **Using the debugger** — breakpoints, stepping, and reading `Locals` / `Watch` instead of guessing
- **Querying the database in SSMS** — checking what is *really* in the tables
- Writing down *why* something broke, not just *that* it works now

---

## 🚀 Getting started

1. Open `code/LibraryApi/LibraryApi.slnx` in Visual Studio.

2. Check the connection string in `LibraryApi/appsettings.json`:

```json
"ConnectionStrings": {
  "LibraryDb": "Server=.\\SQLEXPRESS;Database=LibraryDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

Change `Server=` if your SQL Server is somewhere else (LocalDB users:
`Server=(localdb)\\MSSQLLocalDB`).

3. Create the database. The migration and the seed data are already in the project — you only
   have to apply them, from the **Package Manager Console**:

4. Run the API (F5). Swagger opens.

5. Open **SSMS**, connect to the same server as in your connection string, and confirm the
   `LibraryDb` database is there:

```sql
SELECT * FROM Author;   -- 4 rows
SELECT * FROM Book;     -- 9 rows
```

   Keep this window open for the whole homework. You will need it.


---

## 🔸 The endpoints

| Method | Route | What it should do |
|---|---|---|
| GET | `/api/books` | All books. Optional `?genre=Fantasy` and `?minYear=1950` filters |
| GET | `/api/books/{id}` | One book, 404 if it doesn't exist |
| GET | `/api/books/by-author/{authorId}` | All books by one author, 404 if the author doesn't exist |
| POST | `/api/books` | Create a book, 201 + the created book |
| PUT | `/api/books` | Update a book, 204 |
| DELETE | `/api/books/{id}` | Delete a book, 204 |

---

## 🔎 Debug it — don't guess it

**This is the main skill this homework trains.** Reading code and *thinking* about what it does is
guessing. Putting a breakpoint on the line and *looking* at the value is knowing. Every one of
these bugs takes about a minute with the debugger and can take an hour without it.

A bug report gives you a symptom at the very end of the pipeline — the JSON. Between the database
and that JSON the data crosses four layers:

```text
DB → repository → service → mapper → controller → JSON
```

Start debugging at the very beginning, the Controller.

---

## 🔽 Check the database in SSMS

The debugger tells you what your **code** is doing. SSMS tells you what the **database** actually
holds. You need both, and the interesting bugs are the ones where the two disagree.

Open `LibraryDb` in SSMS and keep the query window next to Visual Studio. Two questions you can
only answer there:

Rule for this homework: **before you blame the code, prove the row.** And after every fix,
check the row again — a response that looks right and a table that looks wrong is still a bug.

---

## 📕 The bug reports

### 🐞 Ticket #1 — "Every book in our library has zero pages"

> `GET /api/books` and `GET /api/books/1` both come back with `"pageCount": 0` for **every**
> book. I checked the `Book` table in SSMS — the page counts are really in there (1984 has 328).
> Nothing else in the response is wrong.

### 🐞 Ticket #2 — "The author's name disappears, but only on one screen"

> `GET /api/books/by-author/1` gives me three books and every one says
> `"authorFullName": "Unknown"`.
> But `GET /api/books` shows `"authorFullName": "George Orwell"` for those exact same books.
> Same books, same field, two different answers.

### 🐞 Ticket #3 — "New books are created with id 0"

> `POST /api/books` answers `201 Created` and gives me back the book I sent — except
> `"id": 0`, and the `Location` header says `/api/Books/0`.
> Then it gets weird: sometimes the book really is in `GET /api/books` afterwards,
> sometimes it isn't. Same request, different result.

### 🐞 Ticket #4 — "Editing a book silently does nothing"

> `PUT /api/books` with a changed title answers `204 No Content`, which means it worked.
> Then I `GET /api/books/1` and the title is exactly what it was before. Nothing changed.
> No error, no exception, no 500. The row in SSMS is untouched too.

### ⭐ Ticket #5 — "The year filter loses a book"

> `GET /api/books?minYear=1949` should give me every book published in 1949 **or later**.
> "1984" was published in 1949 and it is not in the list. Everything from 1950 on is fine.

---

## Requirements

1. **Reproduce first, code second.** Before you change a single line, run all five requests (one by one) and
   record what you actually got back. You cannot tell that you fixed something if you never saw
   it broken.

2. **Find and fix bugs #1 to #5.** 

3. **Verify each fix** by re-running the exact request from step 1 and showing the correct
   response. For the three tickets that touch the database (#3 `POST`, #4 `PUT`, and #1's
   claim about `PageCount`), also run a `SELECT` in SSMS and show that the table agrees with
   the JSON.

4. **Don't break what already works.** When you're done, these must still behave:
   - `GET /api/books` and the `?genre=` filter
   - `DELETE /api/books/{id}` → `204`, and the book is really gone
   - `GET /api/books/999` → `404` with a problem response
   - `POST` with `"pageCount": 0` → `400 "PageCount must be greater than zero."`
   - `POST` with a non-existing `authorId` → `400 "Author with id 99 does not exist."`

5. **Write a `BUG-REPORT.md`** next to the solution folder. For each ticket:

```text
## Ticket #N
- File / method: ...
- How I found it: (where you put the breakpoint and what the value was there)
- What the database said: (the SELECT you ran and what came back — skip only if the ticket never touches the DB)
- What was wrong: (1-2 sentences, in your own words)
- The fix: (the line you changed, before and after)
- How I proved it works: (request + response, and the SELECT again where it applies)
```

This file is the actual homework. A fixed app with no explanation gets no credit — I will ask
you in class why the bug happened, and "the AI said so" is not an answer.

---

# 🤖 AI Guidelines

This homework is the one place where "just ask the AI" ruins the exercise completely. Pasting the
whole solution in and asking *"find the bugs"* will work — and you will have learned nothing,
because the skill being trained here is **narrowing a problem down**, not knowing the answer.

Do it in this order: reproduce → **check the row in SSMS** → guess which layer → **put a breakpoint
there** → fix → verify (response *and* row). Then use AI to check whether your *explanation* is right.

The debugger is the honest version of "ask the AI": it also tells you the answer, except it tells
you the answer about *your* running code, and you are the one who asked the question.