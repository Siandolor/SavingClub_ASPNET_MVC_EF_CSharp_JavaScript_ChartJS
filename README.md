#### SavingClub (ASP.NET MVC + EF + Chart.js)

A structured **ASP.NET Core MVC application** for managing a community savings club.  
Members can record deposits and withdrawals, track balances, and visualize their financial activity through **interactive charts** built with **Chart.js**.  
The app follows a clean MVC architecture and uses **Entity Framework Core (SQLite)** for lightweight, file-based persistence.

---

## Features

- **Members Management**  
  Add, edit, deactivate, or delete members (with optional profile pictures).  
  Deletion is protected when payment data exists.  

- **Payments (Incomes & Expenses)**  
  Log cash movements with amount, date, description, and member link.  
  Filter payments by member, description, or transaction type.  

- **Dashboard & Filters**  
  Quickly view the latest transactions directly from the home screen.  
  Dynamic filtering options: date range, type, or specific member.  

- **Statistics & Visualization**  
  Real-time bar charts powered by **Chart.js**.  
  Top 5 income and expense transactions dynamically rendered from server data.  

- **Responsive Design**  
  Built with Bootstrap for mobile-friendly, clean UI.  

- **SQLite Database**  
  Compact, portable, and automatically created on first run.  

---

## Project Structure

```
SavingClub/
├── Controllers/
│   ├── HomeController.cs          # Dashboard + filtered transactions
│   ├── MembersController.cs       # Member CRUD & image uploads
│   ├── PaymentsController.cs      # Payment CRUD & validation logic
│   └── StatsController.cs         # Aggregated statistics & Chart.js data
│
├── Data/
│   └── AppDbContext.cs            # EF Core context and entity configuration
│
├── Models/
│   ├── Member.cs                  # Member entity (name, image, active flag)
│   ├── Payment.cs                 # Income/Expense with date, description, amount
│   └── NotInFutureAttribute.cs    # Validation attribute for date input
│
├── ViewModels/
│   ├── ErrorViewModel.cs          # Error handling model
│   ├── PaymentFilterViewModel.cs  # Filters for searching and listing payments
│   ├── PaymentsIndexViewModel.cs  # Composite list for payment index view
│   ├── StatsViewModel.cs          # Data structure for statistics view
│   ├── StatsChartItem.cs          # Chart data entry model
│   ├── StatsFilterViewModel.cs    # Filter configuration for stats
│   └── StatsOverviewViewModel.cs  # Summaries for total income, expense, balance
│
├── Views/
│   ├── Home/                      # Dashboard and search views
│   ├── Members/                   # Member CRUD pages
│   ├── Payments/                  # Payment CRUD pages
│   ├── Stats/                     # Interactive chart & summary view
│   └── Shared/                    # Layouts, partials, validation scripts
│
├── wwwroot/
│   ├── css/                       # Bootstrap and custom styles
│   └── js/                        # Front-end logic and Chart.js integration
│
├── Program.cs                     # App configuration and startup pipeline
├── GlobalUsings.cs                # Shared global using directives
├── appsettings.json               # Database and app configuration
└── SavingClub.csproj              # Project definition
```

---

## How It Works

1. **Entity Framework Core Integration**  
   The database context (`AppDbContext`) defines relationships between `Member` and `Payment`.  
   SQLite provides an embedded relational database without external setup.

2. **Member Lifecycle**  
   Members can be activated/deactivated; deletion is restricted if transactions exist.  
   Profile pictures are stored in `/wwwroot/images/members/`.

3. **Payment Validation**  
   Includes custom validation attributes ensuring no future dates and required fields.  

4. **Dynamic Statistics**  
   `/Stats` aggregates payment data, builds server-side summaries, and sends JSON data  
   to Chart.js via AJAX for instant visualization.

5. **Separation of Concerns**  
   Each controller handles one domain area, with typed ViewModels connecting models and views.  

---

## Setup & Installation

### Prerequisites
- .NET 6.0 SDK or newer  
- SQLite (included with .NET)  
- Visual Studio, Rider, or VS Code  

### Build & Run
```bash
  git clone https://github.com/Siandolor/SavingClub_ASPNET_MVC_EF_CSharp_JavaScript_ChartJS.git
  cd SavingClub_ASPNET_MVC_EF_CSharp_JavaScript_ChartJS
  dotnet restore
  dotnet build
  dotnet run
```

Then open:
```
  https://localhost:5001
```

The database (`SavingClub.db`) will be created automatically on first run.

---

## Technical Notes

- **Framework:** ASP.NET Core MVC (.NET 6+)  
- **Database:** SQLite via Entity Framework Core  
- **UI Framework:** Bootstrap 5  
- **Charts:** Chart.js (dynamic JSON feed)  
- **Validation:** DataAnnotations + Custom attribute (`NotInFuture`)  
- **File Uploads:** `IFormFile` handling via `IWebHostEnvironment`  
- **Security:** HTTPS enforced, anti-forgery tokens enabled  

---

## License
Licensed under the **MIT License**.  
© 2025 Daniel Fitz  

---

## Notes
- Educational and demonstrative project — easily portable or extendable.  
- Uses clear comment sections (`// =====`) throughout for maintainability.  
- Focused on simplicity, transparency, and data integrity.  

> “SavingClub – because community wealth grows when managed together.”  
> — Daniel Fitz, 2025
