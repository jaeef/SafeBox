# Database Setup Guide

Step-by-step guide to set up the SafeBox database on your machine.

---

## Step 1 — Install SQL Server

If you don't already have SQL Server installed:

1. Download [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (free)
2. Run the installer → choose **Basic** installation
3. Download [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

---

## Step 2 — Find Your Server Name

1. Open **SSMS**
2. On the connection dialog, note the **Server name** field — it will look something like:
   ```
   YOURPC\SQLEXPRESS
   ```
3. Click **Connect** to verify it works

> Keep this server name handy — you'll need it in Step 4.

---

## Step 3 — Run the Setup Script

1. In SSMS, click **File → Open → File**
2. Navigate to the project and open:
   ```
   Database/SafeBox_Setup.sql
   ```
3. Click **Execute** (or press `F5`)

This creates:
- The `SafeBox` database
- All required tables (`Users`, `Admins`, `Roles`, `Vaults`, `Files`, `ActivityLogs`, `AuditLogs`, `SharedAccess`)
- Default roles (`Admin`, `User`)

---

## Step 4 — Configure the Application

Update the connection string in **two files** — replace `YOUR_SERVER` with the server name from Step 2:

### File 1: `SafeBox/Infrastructure/Data/DatabaseHelper.cs`

```csharp
return @"Data Source=YOUR_SERVER\SQLEXPRESS;Initial Catalog=SafeBox;Integrated Security=True;TrustServerCertificate=True";
```

### File 2: `SafeBox/app.config`

```xml
<add name="SafeBox.Properties.Settings.SafeBoxConnectionString"
     connectionString="Data Source=YOUR_SERVER\SQLEXPRESS;Initial Catalog=SafeBox;Integrated Security=True;TrustServerCertificate=True"
     providerName="System.Data.SqlClient" />
```

---

## Step 5 — Verify

Run the application. On startup, it automatically tests the database connection. If successful, you'll see the Login screen.

If it fails, check:
- SQL Server service is running (open **Services** → look for `SQL Server (SQLEXPRESS)`)
- Server name is correct
- Database `SafeBox` exists in SSMS
- Windows Authentication is enabled on the SQL Server instance

---

## Database Schema

### Tables Overview

| Table | Purpose |
|---|---|
| `Roles` | Stores user roles (Admin, User) |
| `Users` | Registered users with hashed passwords |
| `Admins` | Admin accounts (separate from users) |
| `Vaults` | Encrypted vaults owned by users |
| `Files` | Encrypted files stored within vaults |
| `ActivityLogs` | User action history (upload, download, delete) |
| `AuditLogs` | Admin action audit trail |
| `SharedAccess` | File sharing permissions between users |

### Relationships

```
Roles ──< Users ──< Vaults ──< Files
                                 │
Users ──< ActivityLogs           │
                                 │
Admins ──< AuditLogs    SharedAccess >── Files
                        SharedAccess >── Users (Shared_By)
                        SharedAccess >── Users (Shared_With)
```

### Column Details

Full table definitions with all columns, data types, and constraints are in [`SafeBox_Setup.sql`](SafeBox_Setup.sql).
