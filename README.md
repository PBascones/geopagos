# 🎾 Geopagos Tournament Simulator

This project simulates a single-elimination tennis tournament using object-oriented programming principles and a layered architecture. It was developed as a technical challenge for Geopagos.

---

## 📚 Table of Contents

- [✅ Requirements Met](#✅-requirements-met)
- [🏗️ Project Structure](#️project-structure)
- [🧪 How to Run](#-how-to-run)
- [📡 API Endpoints](#api-endpoints)
- [🧪 Testing](#-testing)
- [⚙️ Additional Features](#️additional-features)
- [🚫 Features Not Implemented](#features-not-implemented)
- [🙋 Author](#-author)

---

## ✅ Requirements Met

### ✅ Object Model
- Clearly defined classes: `Player`, `MalePlayer`, `FemalePlayer`, `Tournament`, `TournamentResult`, `PlayerSnapshot`.
- Uses inheritance, encapsulation, and polymorphism (e.g., `GetEffectiveScore()` method).
- Strong typing via `Gender` enum.

### ✅ Tests
- Complete unit tests for:
  - `TournamentService`: simulation logic, persistence, validations.
  - `TournamentPresenter`: model mapping, error handling.
- Mocking with `Moq`, assertions with `xUnit`.

### ✅ Documentation
- This `README.md` file.
- Semantic naming and code comments in English.

### ✅ Code Style
- Clean, readable code with single responsibility.
- Small, focused classes following SRP.

### ✅ Architecture
- Layered architecture:
  - `Controller → Presenter → Service → Repository`
- Separation of DTOs (`PlayerModel`, `TournamentResultModel`) from entities.
- Uses `DbContext` with SQL Server.
- Clean configuration in `Program.cs`.

### ✅ Business Logic
- Gender-specific logic:
  - Male: `Skill + Strength + Speed`
  - Female: `Skill + ReactionTime`
- Luck factor added to each match to make outcomes more realistic and unpredictable.
- No ties allowed.

### 🧠 Object-Oriented Design

The solution follows core OOP principles:

- Uses **inheritance and polymorphism** (`Player`, `MalePlayer`, `FemalePlayer`)
- Applies **clean architecture** with clear separation of layers
- Follows **SRP** and avoids domain leaks into presentation
- Business logic is **modular, testable, and extensible**

> Designed to be easy to understand, maintain, and scale.

---

## 🏗️ Project Structure

```
Geopagos.sln
├── Geopagos.API/                  → Main WebAPI project
│   ├── Controllers/
│   │   └── TournamentController.cs
│   ├── appsettings.json
│   └── Program.cs

├── Geopagos.Entities/            → Domain entities
│   ├── Business/
│   │   ├── Player.cs
│   │   ├── MalePlayer.cs
│   │   ├── FemalePlayer.cs
│   │   ├── PlayerSnapshot.cs
│   │   └── TournamentResult.cs
│   └── Enums/
│       └── Gender.cs

├── Geopagos.Presenter/           → API models and mappers
│   ├── Models/
│   │   ├── PlayerModel.cs
│   │   └── TournamentResultModel.cs
│   ├── Helpers/
│   │   └── SnapshotMappingExtensions.cs
│   └── TournamentPresenter.cs

├── Geopagos.Repository/          → Database access
│   ├── Migrations/
│   │   └── 2025-04-18 - 01 - initial-script.sql
│   ├── Repositories/
│   │   └── TournamentRepository.cs
│   └── GeopagosDbContext.cs

├── Geopagos.Services/            → Business logic
│   ├── Interfaces/
│   │   └── ITournamentService.cs
│   ├── Base/
│   │   ├── ServiceResponse.cs
│   │   └── ServiceError.cs
│   └── TournamentService.cs

└── Geopagos.Tests/               → Unit tests (xUnit)
    ├── Presenter/
    │   └── TournamentPresenterTests.cs
    └── Services/
        └── TournamentServiceTests.cs
```

---

## 🧪 How to Run

1. Create the `Geopagos` database in SQL Server.
2. Run the SQL scripts to create:
   - `TournamentResult` and `PlayerSnapshot` tables.
3. Update the connection string in `appsettings.json`:
   ```json
   "DefaultConnection": "Server=YOUR_SERVER\SQLEXPRESS;Database=Geopagos;Trusted_Connection=True;TrustServerCertificate=True;"
   ```
4. Run the project using `dotnet run` or Visual Studio.
5. Test endpoints via Swagger at: `https://localhost:{PORT}/swagger`

---

## 📡 API Endpoints

### `GET /api/tournament`
- Returns completed tournaments.
- Optional filters:
  - `from`: start date
  - `to`: end date
  - `gender`: "Male" or "Female"

---

### `POST /api/tournament`
- Simulates a tournament with submitted players.
- Requires the number of players to be a power of 2.
- Example input:

```json
[
  {
    "name": "Paola",
    "skillLevel": 95,
    "gender": "Female",
    "reactionTime": 90
  },
  {
    "name": "Beatriz",
    "skillLevel": 88,
    "gender": "Female",
    "reactionTime": 92
  }
]
```

---

## 🧪 Testing

You can use the following sample JSONs to test the `POST /api/tournament` endpoint directly from Swagger or Postman.

### 🏆 Female Tournament (2 players)

```json
[
  {
    "name": "Paola",
    "skillLevel": 95,
    "gender": "Female",
    "reactionTime": 90
  },
  {
    "name": "Gladys",
    "skillLevel": 88,
    "gender": "Female",
    "reactionTime": 92
  }
]
```

### 🏆 Male Tournament (4 players)

```json
[
  {
    "name": "Juan",
    "skillLevel": 80,
    "gender": "Male",
    "strength": 90,
    "speed": 85
  },
  {
    "name": "Luis",
    "skillLevel": 75,
    "gender": "Male",
    "strength": 88,
    "speed": 82
  },
  {
    "name": "Carlos",
    "skillLevel": 82,
    "gender": "Male",
    "strength": 86,
    "speed": 89
  },
  {
    "name": "Andres",
    "skillLevel": 78,
    "gender": "Male",
    "strength": 85,
    "speed": 83
  }
]
```

### 📥 Testing via `curl`

You can also test the tournament retrieval using the following curl command:

```bash
curl -X GET "https://localhost:7028/api/tournament?from=2025-04-19&to=2025-04-21&gender=Male" -H "accept: application/json"
```

Replace the port if needed



## ⚙️ Additional Features

- 🛠️ SQL Server persistence (non-embedded)
- 🎯 DTOs separated from entities
- 🔄 `ToModel()` mappers to prevent serialization cycles
- 🧱 Structured error handling via `ServiceResponse`
- 🔍 Unit testing with mocks and clean assertions

---

## 🚫 Features Not Implemented

- Integration tests (intentionally skipped for scope clarity)
- Deployment to Azure / Docker

> Integration tests were intentionally skipped in this stage to keep the focus on core business logic, architecture, and testability. The core logic is thoroughly covered with unit tests.

---

## 🙋 Author

**Pablo Bascones Busch**  
Systems Engineer who built this project fueled by chocolate, curiosity, and the hope of joining Geopagos very soon ☕🚀

---

```markdown
Thanks for reviewing this challenge – I look forward to hearing from you!