# Notes App — Hosting on IIS 🌐

This repo contains two applications:

| App | What it is | Technology |
|---|---|---|
| **Notes API** | The backend — notes, authentication, database access | ASP.NET Core 8 |
| **Notes SPA** | The frontend — login screen and notes board | Static HTML/CSS/ES modules |

In class you run both on your own machine with the development tooling. This
document explains **what hosting is and why it is different**. When you are ready
to actually do it, follow [`HOSTING-IIS.md`](HOSTING-IIS.md), which is the
step-by-step guide.

Read this page first. Almost every error in the step guide is a consequence of
one of the ideas below, and the fixes make very little sense without them.

---

## 🔸 What hosting and deploying actually mean

Up to now you have been *running* the app. From here on you are *hosting* it.
They are different activities, and that difference is the whole subject.

### How you have been working

| | Notes API | Notes SPA |
|---|---|---|
| Started by | `dotnet run` (or F5) | Live Server in VS Code |
| Web server | **Kestrel**, built into the app | Live Server's dev server |
| Lives as long as | your terminal / debugger session | VS Code stays open |
| Runs as | **you** — your Windows account | you |
| Address | `https://localhost:7240` | `http://127.0.0.1:5500` |
| Source code | present, recompiled on change | served straight off disk |

This is a *development* setup. The server exists to give you a fast feedback
loop, and it borrows your identity and your session to do it.

### What hosting means

Hosting means a **long-running web server process, owned by the operating
system, serves your app to anyone who asks** — whether or not you are logged in,
whether or not you have a terminal open. It starts with Windows and stays up.

On Windows that server is **IIS** (Internet Information Services). On Linux you
would usually put nginx in front of Kestrel; on Azure it is App Service, which is
IIS underneath. The concepts transfer; only the tooling changes.

---

## 🔸 Four words that get used interchangeably (they are not)

- **Build** — compile source into DLLs. Output is for *your* machine, lands in
  `bin\Debug\`, and still expects the whole project around it.
- **Publish** — `dotnet publish` produces a **self-contained folder of exactly
  what is needed to run**: your DLLs, your dependencies, `appsettings.json`, and
  a generated `web.config`. No source, no project files, Release configuration.
  This is the *artifact*.
- **Deploy** — get that artifact onto the server and tell the server about it.
  In this guide, deploying is a copy into `C:\inetpub\` plus some IIS setup.
- **Host** — the ongoing business of serving it: listening on a port, starting
  the app when the first request arrives, restarting it when it crashes.

You will publish and deploy many times. You configure hosting once.

---

## 🔸 What a web server actually does

A request arrives at port 443 for `notesapi.test`. The server has to:

1. **Accept the connection and terminate TLS** — decide which certificate to
   present. This is why the guide has you create one.
2. **Decide which site it is for.** One machine, many sites: the server matches
   on IP + port + hostname. That triple is called a **binding**. Two sites can
   share port 443 because the hostname travels in the TLS handshake, a mechanism
   called **SNI** (Server Name Indication).
3. **Serve it, or hand it off.** For `index.html` or `app.js`, IIS reads the file
   off disk and returns it — that is all the SPA site ever does. For `/api/notes`
   there is no such file, so IIS passes the request to your application.

---

## 🔸 How IIS runs an ASP.NET Core app

IIS understands .NET Framework natively; it does **not** understand .NET 8. The
bridge is the **ASP.NET Core Module**, installed by the Hosting Bundle.

IIS groups sites into an **application pool**, and each pool gets a worker
process, `w3wp.exe`. The module starts your app *inside* that worker process —
this is **in-process hosting**, the default since .NET Core 3.0 — and forwards
requests to it. Two consequences you will meet later:

- `w3wp.exe` holds your DLLs open while the site is running, which is why
  republishing needs an `app_offline.htm`.
- If your app throws during startup, the worker dies before it can tell you
  anything useful. You get a bare **500.30** and have to read the Windows event
  log to find out what actually happened.

### The one idea that explains most of the errors

The pool also chooses an **identity** to run as:

> ⚠ **Under IIS your app is not you.** It runs as a virtual account named
> `IIS APPPOOL\NotesApi`. That account has never opened your SQL Server, has no
> rights to your folders, and inherits nothing you set up for yourself.

The folder-permissions step and the SQL-login step exist entirely because of
this, and most of the 500s in the troubleshooting table trace back to it.

### The second idea: you are now in Production

A published app defaults to the **Production** environment. That means
`appsettings.Development.json` is not loaded, Swagger is usually off, and the
developer exception page is replaced by a blank error page. Configuration that
worked under Kestrel can simply not be there any more. The step guide has a
section on this.

---

## 🔸 What this is not

This is a **production-shaped setup on your own machine**: real hostnames, real
HTTPS, real service identities — but a self-signed certificate and a local
database. It is for learning the moving parts, not for putting on the internet.

---

## 🔸 Version note

This guide targets **.NET 8**. Note that .NET 8 and .NET 9 both reach end of
support on **10 November 2026**; .NET 10 is the current LTS. Nothing here changes
conceptually on a newer version — you would install a different Hosting Bundle
and retarget the project.