<p align="center">
  <img src="Icons/iconBig.jpg" alt="SafeBox" width="140"/>
</p>

<h1 align="center">SafeBox — Secure File Vault Manager</h1>

<p align="center">
  A Windows desktop application for encrypting, storing, and managing files in password-protected vaults.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET%20Framework-4.8-blue?logo=dotnet"/>
  <img src="https://img.shields.io/badge/Platform-Windows-lightgrey?logo=windows"/>
  <img src="https://img.shields.io/badge/Database-SQL%20Server-red?logo=microsoftsqlserver"/>
  <img src="https://img.shields.io/badge/Encryption-AES--256-brightgreen"/>
</p>

---

## Features

**User Side**
- Create encrypted vaults and upload files — automatically encrypted with AES-256
- Download files with on-the-fly decryption
- Live file search across all vaults
- Activity log tracking every upload, download, and deletion
- Profile management with password recovery via security hints

**Admin Side**
- System-wide dashboard with user stats
- User management — search, activate, deactivate accounts
- Comprehensive audit log of all admin actions
- Role management

---

## Architecture

Built with **N-Tier (Layered) Architecture**:

| Layer | Responsibility |
|---|---|
| **Domain** | Core entities (User, Vault, File, Admin) and custom exceptions |
| **Application** | Service interfaces, business logic, DTOs, session management |
| **Infrastructure** | SQL Server repositories, AES encryption, password hashing |
| **Presentation** | WinForms UI — Forms, UserControls, Admin panel |

---

## Tech Stack

| Component | Technology |
|---|---|
| Language | C# 7.3 |
| Framework | .NET Framework 4.8 |
| UI | WinForms + [ReaLTaiizor](https://github.com/Developer-Mak/ReaLTaiizor) + [CuoreUI](https://www.nuget.org/packages/CuoreUI.Winforms/) |
| Database | SQL Server (Express / Developer) |
| Encryption | AES-256 |
| Password Hashing | Salted cryptographic hashing |

---

## Prerequisites

- **Windows 10/11**
- **Visual Studio 2022** with `.NET desktop development` workload and `.NET Framework 4.8 Targeting Pack`
- **SQL Server** (Express or Developer edition)
- **SSMS** (SQL Server Management Studio)

---

## Setup

### 1. Clone

```bash
git clone https://github.com/jaeef/SafeBox.git
```

### 2. Database Setup

Follow the guide in **[`Database/SETUP_GUIDE.md`](Database/SETUP_GUIDE.md)** — it walks you through:
- Installing SQL Server
- Finding your server name
- Running the setup script (`Database/SafeBox_Setup.sql`)
- Configuring the connection string

### 3. Restore NuGet Packages

In Visual Studio: **Right-click Solution → Restore NuGet Packages**

### 4. Build & Run

Press `F5`. The app tests the database connection on startup and opens the Login screen.

---

## Usage

1. **Register** a new account from the login screen
2. **Login** → lands on the User Dashboard
3. **Create a vault** → upload files (encrypted automatically)
4. **Download** files (decrypted on-the-fly)
5. **Admin login** → access User Management, Audit Logs, and Roles

---

## Security

| Mechanism | Detail |
|---|---|
| Password Storage | Salted hashing — plain-text never stored |
| File Encryption | AES-256 before database storage |
| SQL Injection | All queries use parameterized `SqlCommand` |
| Audit Trail | Every admin action logged with timestamp |

---

## Team

| Avatar | Contributor | GitHub |
|---|---|---|
| ![jaeef](https://github.com/jaeef.png?size=50) | Abu Saleh Mohammad Jaeef | [@jaeef](https://github.com/jaeef) |
| ![ruh-n](https://github.com/ruh-n.png?size=50) | MD. NAFIZ IQBAL ROHAN | [@i-ruh-n]([https://github.com/ruh-n](https://github.com/i-ruh-n)) |
| ![alamin0226](https://github.com/alamin0226.png?size=50) | Md. Alamin | [@alamin0226](https://github.com/alamin0226) |


---

## Contributing

1. Fork the repo
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Commit with descriptive messages
4. Push and open a Pull Request
