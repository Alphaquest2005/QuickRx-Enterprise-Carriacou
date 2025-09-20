# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

Build the entire solution:
```bash
msbuild QuickRx-Enterprise-Carriacou.sln /p:Configuration=Debug /p:Platform="Mixed Platforms"
msbuild QuickRx-Enterprise-Carriacou.sln /p:Configuration=Release /p:Platform="Mixed Platforms"
```

Restore NuGet packages:
```bash
nuget restore QuickRx-Enterprise-Carriacou.sln
```

Build specific projects:
```bash
msbuild QuickSales/QuickSales.csproj /p:Configuration=Debug /p:Platform=x86
msbuild RMSDataAccessLayer/RMSDataAccessLayer.csproj /p:Configuration=Debug
```

## High-Level Architecture

QuickRx Enterprise is a **pharmacy Point of Sale (POS) system** built for Hills and Valley Pharmacy using WPF and Microsoft Prism framework for modular architecture.

### Technology Stack
- **.NET Framework 4.6.1** with WPF for UI
- **Microsoft Prism 4.1** for modular application framework
- **Entity Framework 6.1.3** with Database-First approach
- **Unity 2.1** for dependency injection
- **TrackableEntities 2.5.2** for change tracking
- **SQL Server** database (`QuickSales-Enterprise-Carriacou`)

### Core Applications
- **QuickSales** - Main POS application with Shell.xaml as primary window
- **QuickSalesManager** - Management interface for reporting and administration
- **QuickBooks** - QuickBooks integration module for financial synchronization

### Prism Module Architecture
The application uses Prism regions for UI composition:

```
Shell.xaml (Main Container)
├── HeaderRow - Application header
├── Body
│   ├── LeftRegion - Search/navigation (SuppView)
│   └── CenterRegion - Main sales interface (SalesView)
├── TransactionView - Transaction display and management
└── Footer - Controls and logout functionality
```

**Key Prism Modules:**
- `SalesRegionModule` - Core sales functionality in `/Regions/SalesRegion/`
- `TransactionModule` - Transaction management in `/Regions/Transaction/`
- `LeftRegionModule` - Search and navigation in `/Regions/LeftRegion/`

### Data Access Layer

**RMSDataAccessLayer** contains all Entity Framework models and business entities:

- **Database-First EF** with `.edmx` files and T4 code generation
- **Custom entity classes** in `/CustomClasses/` extend generated entities
- **TrackableEntities integration** for comprehensive change tracking

**Key Entity Categories:**
- **People:** `Person`, `Cashier`, `Patient`, `Doctor`
- **Pharmacy:** `Prescription`, `PrescriptionEntry`, `Medicine`, `Item`
- **Sales:** `TransactionBase`, `TransactionEntry`, `Batch`
- **Inventory:** `StockItem`, `InventoryItem`

### Supporting Libraries

- **Common.Core** - Shared utilities, logging infrastructure, and RelayCommand
- **BarCodes** - Barcode generation and processing (UPC-A support)
- **ACTB (Aviad.WPF.Controls)** - Custom WPF controls including AutoCompleteTextBox
- **SUT.PrintEngine** - Comprehensive printing system with multiple output formats

### Configuration Management

**Database Connections:**
- Primary: `QuickSales-Enterprise-Carriacou` on `MINIJOE\SQLDEVELOPER2022`
- Connection strings in `app.config` with Entity Framework metadata

**Application Settings:**
- QuickBooks company file configuration
- Server mode settings (currently `False` for local operation)
- log4net configuration with file and email appenders

### Development Patterns

**MVVM Implementation:**
- SimpleMvvmToolkit for ViewModels with property change notification
- RelayCommand for command binding throughout the application
- Prism EventAggregator for loose coupling between modules

**Dependency Injection:**
- Unity container configured in Shell application
- Service registration occurs in individual Prism modules
- Interface-based programming with repository patterns

**Change Tracking:**
- TrackableEntities provides client-side entity state management
- Supports CRUD operations with automatic state tracking
- Repository pattern implementation for data access

### Logging Infrastructure

**log4net Configuration:**
- File logging to `QBSales-Log.txt` with minimal locking
- Email alerts for ERROR level and above via SMTP
- Configurable logging levels (DEBUG, INFO, WARN, ERROR, FATAL)

### Project Structure Conventions

- **Regions** contain UI modules that plug into the Shell
- **CustomClasses** extend Entity Framework generated entities
- **ViewModels** follow MVVM pattern with appropriate data binding
- **Services** provide business logic and external integrations
- **Properties** contain assembly info and application settings

### Database Schema Notes

Key tables: `Persons`, `Cashiers`, `Patients`, `Doctors`, `Prescriptions`, `TransactionBase`, `Items`, `Medicines`, `Batches`, `CashierLogs`. The schema supports comprehensive pharmacy operations including prescription management, inventory tracking, and sales transactions.

### QuickBooks Integration

The system integrates with QuickBooks POS through XML-based data exchange for inventory adjustments, sales receipts, and customer management. Configuration is managed through application settings.