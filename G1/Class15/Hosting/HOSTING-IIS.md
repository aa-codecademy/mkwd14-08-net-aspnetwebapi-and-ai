# Hosting the Notes App on IIS - step by step 🌐

How to run the Notes API and the Notes SPA on this machine under IIS, on friendly
hostnames with HTTPS and no port numbers:

| App | URL | Physical path |
|---|---|---|
| Notes API | `https://notesapi.test` | `C:\inetpub\NotesApi` |
| Notes SPA | `https://notesapp.test` | `C:\inetpub\NotesAppWeb` |

Both sites share port 443 using SNI.

Everything below is **elevated** (PowerShell as Administrator, or IIS Manager as
Administrator) unless it says otherwise.

---

## 🔸 1. Install IIS

```powershell
Enable-WindowsOptionalFeature -Online -All -FeatureName `
  IIS-WebServerRole, IIS-WebServer, IIS-StaticContent, IIS-DefaultDocument, `
  IIS-WebServerManagementTools, IIS-ManagementConsole
```

Or GUI: *Turn Windows features on or off* → **Internet Information Services** →
tick **Web Management Tools → IIS Management Console** and **World Wide Web
Services → Common HTTP Features → Static Content + Default Document**.

Check: `Get-Service W3SVC, WAS` — both should be **Running**.

## 🔸 2. Install the ASP.NET Core 8 Hosting Bundle

Download from <https://dotnet.microsoft.com/download/dotnet/8.0> →
**ASP.NET Core Runtime 8.0.x → Hosting Bundle (Windows)**.

Two traps:

- **winget cannot do this.** There is no hosting-bundle package. The similarly
  named `Microsoft.DotNet.AspNetCore.8` is the plain runtime and does **not**
  contain the IIS module — install that instead and you get a 500.19 with no clue.
- **Install IIS first.** The bundle registers its module into IIS; run it before
  step 1 and you have to repair it afterwards.

Then let IIS pick the module up:

```powershell
net stop was /y
net start w3svc
```

Check: `C:\Program Files\IIS\Asp.Net Core Module\V2\aspnetcorev2.dll` exists.

## 🔸 3. Host names

Open `C:\Windows\System32\drivers\etc\hosts` in an **elevated** Notepad and append:

```text
127.0.0.1  notesapi.test
127.0.0.1  notesapp.test
```

Check: `ping notesapi.test` answers from `127.0.0.1`.

> **Use `.test`, not `.local`.** `.local` is reserved for mDNS and Windows can add
> a resolution delay on it. `.test` is reserved by RFC 2606 for exactly this.

## 🔸 4. Certificate

The .NET dev certificate only covers `localhost`, so it is useless for these names.
Make one that covers both:

```powershell
$cert = New-SelfSignedCertificate -DnsName "notesapi.test","notesapp.test" `
  -CertStoreLocation "Cert:\LocalMachine\My" `
  -FriendlyName "NotesApp Local Dev" -NotAfter (Get-Date).AddYears(5)

# Trust it.
$store = [System.Security.Cryptography.X509Certificates.X509Store]::new("Root","LocalMachine")
$store.Open("ReadWrite"); $store.Add($cert); $store.Close()
```

⚠ **Do not skip the trust step.** An untrusted certificate makes `fetch()` fail,
and the browser console reports it as a **CORS error**. You will chase the wrong
problem for an hour.

## 🔸 5. Code changes

Three edits. Nothing else in the solution is environment-specific.

**5a — `Class15/NotesAppWeb/js/config.js`**

```javascript
//export const API_BASE_URL = "https://localhost:7240";   // Kestrel + Live Server
export const API_BASE_URL = "https://notesapi.test";      // IIS
```

**5b — `NotesApp/NotesApp/Program.cs`** — *add* the origin, do not replace, so the
classroom setup keeps working:

```csharp
policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500",  // Live Server
                   "https://notesapp.test")                           // IIS
      .AllowAnyMethod()
      .AllowAnyHeader();
```

**5c — `NotesApp/NotesApp.Helpers/LoggingHelper.cs`** — comment out the
**MSSqlServer sink** (and the `connectionString` line it is the only user of).

This one is not optional, and it is worth understanding:

```csharp
.WriteTo.MSSqlServer(
    connectionString: connectionString,
    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
    ...)
```

`AutoCreateSqlTable` connects to SQL Server **while the logger is being built**,
which is the very first thing `Program.cs` does. Under IIS the identity is
`IIS APPPOOL\NotesApi`, not you — so one missing SQL permission takes the whole
application down with **HTTP 500.30 before a single line of our code runs**. The
file sinks keep working; only the database sink goes.

The sink is fine under Kestrel, where the app runs as you. 

## 🔸 6. Publish and copy

```powershell
Set-Location C:\<your-location-to-notes-app>\NotesApp
dotnet publish NotesApp\NotesApp.csproj -c Release -o C:\inetpub\NotesApi

xcopy /E /I /Y "C:\<your-location-to-notes-web>\NotesAppWeb\*" "C:\inetpub\NotesAppWeb\"
```

⚠ **Use the absolute source path.** A relative one resolves only from the repo
root, and the publish command above leaves you in `...\NotesApp` — where
xcopy silently matches nothing and reports `0 File(s) copied`.

You should see **8 File(s) copied**. `dotnet publish` writes `web.config` for you;
never hand-write one.

## 🔸 7. Folder permissions

`C:\inetpub\wwwroot` grants IIS read access, but a **sibling folder does not** — it
inherits from `C:\inetpub`, which has no `IIS_IUSRS` entry. Without this the API
returns 500 and the SPA returns 403.

```powershell
icacls "C:\inetpub\NotesApi"    /grant "IIS_IUSRS:(OI)(CI)(RX)" /T
icacls "C:\inetpub\NotesAppWeb" /grant "IIS_IUSRS:(OI)(CI)(RX)" /T
```

Optional, and worth it — lets you republish from a normal terminal:

```powershell
icacls "C:\inetpub\NotesApi"    /grant "<your-machine-name>\<your-user>:(OI)(CI)(M)"
icacls "C:\inetpub\NotesAppWeb" /grant "<your-machine-name>\<your-user>:(OI)(CI)(M)"
```

## 🔸 8. Application pool

IIS Manager → **Application Pools** → right-click → **Add Application Pool**

| Field | Value |
|---|---|
| Name | `NotesApi` |
| **.NET CLR version** | **No Managed Code** |
| Managed pipeline mode | Integrated |
| Start immediately | ✓ |

**No Managed Code** is the setting everyone gets wrong. .NET 8 does not use the
.NET Framework CLR that IIS is offering; the ASP.NET Core Module hosts it instead.

## 🔸 9. The two sites

**Sites** → right-click → **Add Website**, twice:

| Field | API site | SPA site |
|---|---|---|
| Site name | `NotesApi` | `NotesAppWeb` |
| Application pool | `NotesApi` | `DefaultAppPool` |
| Physical path | `C:\inetpub\NotesApi` | `C:\inetpub\NotesAppWeb` |
| Type | https | https |
| IP address | All Unassigned | All Unassigned |
| Port | 443 | 443 |
| **Host name** | `notesapi.test` | `notesapp.test` |
| **Require Server Name Indication** | ✓ | ✓ |
| SSL certificate | NotesApp Local Dev | NotesApp Local Dev |

SNI is what lets two sites share port 443. If IIS refuses the binding, the
**Default Web Site** has a non-SNI 443 binding — remove it.

The SPA site needs no special pool: nothing executes, IIS just hands out files.
It serves `.js` as `text/javascript`, which ES modules require.

## 🔸 10. SQL Server login

The connection string uses `Trusted_Connection=True`. Under IIS that identity is
`IIS APPPOOL\NotesApi` — a virtual account SQL Server has never seen.

```sql
CREATE LOGIN [IIS APPPOOL\NotesApi] FROM WINDOWS;

USE NotesAppDb;
CREATE USER [IIS APPPOOL\NotesApi] FOR LOGIN [IIS APPPOOL\NotesApi];
ALTER ROLE db_datareader ADD MEMBER [IIS APPPOOL\NotesApi];
ALTER ROLE db_datawriter ADD MEMBER [IIS APPPOOL\NotesApi];
```

Run `dotnet ef database update` **as yourself** first if the database is not
current — those two roles cannot create tables, and a published app never migrates.

## 🔸 11. Verify

1. `https://notesapi.test/api/notes` → **401**.
   That is success: the app started and `[Authorize]` answered. Anything in the
   500 family means the app did not start — see the table below.
2. `https://notesapp.test` → the login screen, **no certificate warning**.
3. Log in as `bob` / `SuperSecret123` → the notes board.

---

# 🔽 Redeploying

`w3wp.exe` holds the DLLs while the site is running, so a plain republish fails
with `MSB3027 ... The file is locked by: "w3wp.exe"`.

Drop an `app_offline.htm` in the app root: the ASP.NET Core Module sees it, shuts
the application down, releases the files, and serves that page to anyone who asks.

```powershell
Set-Content "C:\inetpub\NotesApi\app_offline.htm" -Encoding utf8 `
  -Value "<html><body><h1>Deploying...</h1></body></html>"

Set-Location C:\<your-location-to-notes-app>\NotesApp
dotnet publish NotesApp\NotesApp.csproj -c Release -o C:\inetpub\NotesApi

Remove-Item "C:\inetpub\NotesApi\app_offline.htm"
```

The SPA has no such problem — static files are not locked. Just re-run the xcopy.

---

# ⚠ Troubleshooting

| Symptom | Cause |
|---|---|
| **CORS error in the console** | Usually **not** CORS. Open the API URL directly — an IIS error page carries no `Access-Control-Allow-Origin`, so the browser blames CORS for what is really a 500. Fix the 500. |
| **500.30** app failed to start | The app crashed during startup. Almost always the SQL login (step 10) or the MSSqlServer sink (step 5c). |
| **500.19** configuration error | Hosting Bundle missing, or installed before IIS. |
| **500.31** failed to find runtime | App pool is not **No Managed Code**, or the wrong bundle version. |
| **502.5** process failure | Same family as 500.30. |
| **403** on the SPA | `IIS_IUSRS` permissions, step 7. |
| Certificate warning / "Failed to fetch" | Certificate not in Trusted Root, step 4. |
| `MSB3027 ... locked by w3wp.exe` | See "Redeploying". |
| `0 File(s) copied` | Relative xcopy path, step 6. |
| Publish fails: access denied | `C:\inetpub` needs elevation, or grant yourself Modify (step 7). |

## Reading the real error

The IIS error page tells you almost nothing. The actual exception goes to the
Windows Application event log:

```powershell
Get-WinEvent -FilterHashtable @{LogName='Application'; StartTime=(Get-Date).AddMinutes(-10)} |
  Where-Object { $_.ProviderName -match 'IIS AspNetCore|\.NET Runtime' } |
  Select-Object -First 3 TimeCreated, Message | Format-List
```

That is what turned "blocked by CORS policy" into
`Login failed for user 'IIS APPPOOL\NotesApi'` in about ten seconds.

Alternatively set `stdoutLogEnabled="true"` in `C:\inetpub\NotesApi\web.config`
and read `C:\inetpub\NotesApi\logs\stdout*.log`. Turn it back off afterwards — it
grows without limit.

---

