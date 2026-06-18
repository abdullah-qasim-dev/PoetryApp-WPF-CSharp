# Sheher-e-Sukhan 🎭
### A Desktop Platform for Exploring Urdu & Punjabi Poetry

![Platform](https://img.shields.io/badge/Platform-Windows-blue)
![Language](https://img.shields.io/badge/Language-C%23-green)
![Framework](https://img.shields.io/badge/Framework-WPF-purple)
![Database](https://img.shields.io/badge/Database-SQL%20Server-red)

---

## 📖 Overview

**Sheher-e-Sukhan** (Poet's Sanctuary) is a desktop application built with **WPF (C#)** and **SQL Server**, designed to provide an interactive platform for exploring Urdu and Punjabi poetry. Users can discover poets, read their works, submit their own poems, and manage their profiles — all through a clean, user-friendly interface.

> Developed as a semester project at **FAST NUCES Lahore** (Fundamentals of Software Engineering).

---

## ✨ Features

### 👤 User Features
- **User Registration & Login** — Secure authentication with role-based access control
- **Preference Selection** — Users select favourite poets during signup for a personalized experience
- **Main Page** — Browse and explore Urdu & Punjabi poetry content
- **Poet Search & Profiles** — Search poets by name/theme and view detailed biographies
- **Poet Timeline** — View a timeline of each poet's life events and major literary works
- **Poetry Library** — Categorized collection including Ghazals, Nazms, and thematic poems
- **Poem Submission** — Users can submit and share their own poems
- **User Dashboard** — View saved poems, preferences, and recent activity
- **User Profile** — Manage personal details and view followers/following

### 🔧 Admin Features
- **Admin Dashboard** — View and manage registered users
- **User Management** — View user information and delete users from the platform
- **Poetry Database Management** — Add, edit, and organize poets and poems

---

## 🛠️ Tech Stack

| Component | Technology |
|-----------|-----------|
| Language | C# |
| UI Framework | WPF (Windows Presentation Foundation) |
| Database | SQL Server |
| IDE | Visual Studio |
| Architecture | XAML + Code-behind |

---

## 🗃️ Database Schema

| Table | Description |
|-------|-------------|
| `Users` | User accounts, credentials, and roles |
| `Admin` | Admin accounts and credentials |
| `Poets` | Poet profiles, biographies, eras, and image paths |
| `Poetry` | Poems with type (Ghazal, Nazm, etc.), theme, and language |
| `UserPreferences` | User-poet preference mappings (selected at signup) |
| `Categories` | Poetry categories |

---

## 🚀 Getting Started

### Prerequisites
- Windows OS
- Visual Studio (2019 or later)
- SQL Server (LocalDB or full instance)
- .NET Framework

### Setup
1. Clone the repository
   ```bash
   git clone https://github.com/abdullah-qasim-dev/PoetryApp-WPF-CSharp.git
   ```
2. Open `SE project 2025.sln` in Visual Studio
3. Update the connection string in `App.config` to point to your SQL Server instance
4. Build and run the project (F5)

---
