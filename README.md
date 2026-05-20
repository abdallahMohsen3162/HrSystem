# HrSystem — HR Management System

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

---

## Setup & Run

### Step 1 — Clone the Repository

```bash
git clone <repository-url>
cd HrSystem
```

---

### Step 2 — Restore Dependencies

```bash
dotnet restore
```

---

### Step 3 — Run the Project

```bash
cd HrSystem
dotnet watch run --environment Development
```

Then open your browser at:

```
https://localhost:7177
```

> If you see an SSL warning, click **Advanced → Proceed** to continue.

---

> **Note:** The database, all migrations, and environment settings (`appsettings.json`) are already configured and included in the project — no extra setup required.

---

## Default Admin Account

| Field    | Value             |
|----------|-------------------|
| Email    | Admin@yopmail.com |
| Password | 123456            |
| Role     | SuperAdmin        |

---

## Project Structure

```
HrSystem/
├── HrSystem/           # Main project (Controllers, Views, Program.cs)
├── BusinessLayer/      # Business logic layer (Services, Logic)
├── DataLayer/          # Data layer (DbContext, Entities, Migrations)
└── HrSystem.sln        # Solution file
```