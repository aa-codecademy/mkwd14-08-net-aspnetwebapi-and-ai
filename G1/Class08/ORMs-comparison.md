# 🗄️ .NET Data Access: EF Core vs Dapper vs ADO.NET

Only EF Core is a real ORM. Dapper is a micro-ORM (mapper only), and ADO.NET is the raw data access layer that both of the others are built on top of.

---

## 🔧 ADO.NET

The base API: `SqlConnection`, `SqlCommand`, `SqlDataReader`, `DataTable`. You write SQL, you read columns by index or name, you map to objects yourself.

### ✅ Pros
- Fastest possible, no abstraction overhead
- Zero dependencies, full control over connections, transactions, command behavior
- Direct access to provider-specific features (bulk copy, table-valued parameters, streaming large blobs)

### ❌ Cons
- Very verbose — a lot of boilerplate per query
- Manual mapping is error-prone and tedious to maintain
- Easy to leak connections or forget parameterization if you're careless

### 👉 Use it when
Bulk operations (`SqlBulkCopy`), streaming huge result sets, infrastructure code where you can't take dependencies, or when you need something exotic the abstractions don't expose.

---

## ⚡ Dapper

A thin extension over ADO.NET connections. You still write the SQL; Dapper handles parameter binding and maps the result rows to your objects.

### ✅ Pros
- Performance within a few percent of raw ADO.NET
- Removes almost all mapping boilerplate while keeping SQL fully under your control
- Tiny learning curve, easy to add to an existing ADO.NET codebase incrementally
- Excellent for complex queries, reporting, and hand-tuned SQL

### ❌ Cons
- No change tracking, no unit of work, no migrations, no lazy loading
- SQL lives in strings — refactoring a column name won't break compilation
- Writes (insert/update/delete) are all manual; multi-table graph updates get painful
- No built-in database portability

### 👉 Use it when
Read-heavy services, reporting and dashboards, APIs where latency matters, or any codebase where the team is comfortable owning the SQL. Also common as a companion to EF Core for the queries EF generates poorly.

---

## 🏗️ Entity Framework Core

Full ORM: LINQ queries translated to SQL, change tracking, migrations, relationship management.

### ✅ Pros
- Big productivity gain for CRUD and domain-model-driven code
- Compile-time-checked LINQ queries; rename a property and the query breaks at build time
- Change tracking and `SaveChanges` handle multi-entity updates and transactions for you
- Migrations give you versioned schema evolution
- Provider model covers SQL Server, PostgreSQL, SQLite, MySQL, and others

### ❌ Cons
- Slowest of the three; more allocation and startup cost
- Generated SQL can be poor for complex queries — watch for N+1 problems and accidental client-side evaluation
- The abstraction leaks: you eventually have to understand both LINQ translation and the SQL underneath
- Heavier learning curve, and a long-lived `DbContext` can cause memory and staleness problems

### 👉 Use it when
Line-of-business apps, admin backends, anything with a rich domain model and lots of write operations, or teams that want migrations and less SQL. Use `AsNoTracking()` for read-only queries and compiled queries on hot paths.

---

## 📊 At a glance

| | ADO.NET | Dapper | EF Core |
|---|---|---|---|
| Type | Raw data access | Micro-ORM | Full ORM |
| Who writes the SQL | You | You | Generated from LINQ |
| Object mapping | Manual | Automatic | Automatic |
| Change tracking | No | No | Yes |
| Migrations | No | No | Yes |
| Performance | Best | Near-best | Good, with care |
| Boilerplate | High | Low | Lowest |
| Learning curve | Moderate | Low | High |

---

## 🎯 Practical guidance

The most common good setup in production .NET is **EF Core for writes and domain logic, Dapper for heavy or awkward reads**. They share the same `DbConnection`, so you can call Dapper on `context.Database.GetDbConnection()` inside the same transaction. Reach for raw ADO.NET only for bulk loads or streaming.

If you're picking just one: EF Core if the app is write-heavy and model-driven, Dapper if it's read-heavy and you care about latency.