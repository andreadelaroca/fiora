# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

Fiora is an ASP.NET Core 9.0 MVC web application for managing a floral arrangement business. The system handles customer orders, inventory management, admin operations, and floral arrangement catalog with seasonal availability.

**Technology Stack:**
- ASP.NET Core 9.0 (MVC pattern)
- Entity Framework Core with SQL Server (LocalDB)
- ASP.NET Identity for authentication
- Razor views (Spanish UI)

**Database:** SQL Server LocalDB with connection string in `appsettings.json`

## Common Commands

### Build and Run
```powershell
# Build the project
dotnet build

# Run the application
dotnet run

# Run with hot reload
dotnet watch run

# Clean build artifacts
dotnet clean
```

### Database Operations
```powershell
# Add a new migration
dotnet ef migrations add <MigrationName>

# Update database to latest migration
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# Drop database (for clean slate)
dotnet ef database drop
```

### Testing and Scaffolding
```powershell
# Scaffold a new controller with views
dotnet aspnet-codegenerator controller -name <ControllerName> -m <ModelName> -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

# Example: Scaffold Cliente controller
dotnet aspnet-codegenerator controller -name ClientesController -m Cliente -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
```

### Package Management
```powershell
# Restore packages
dotnet restore

# Add a package
dotnet add package <PackageName>

# Update all packages
dotnet list package --outdated
```

## Architecture

### Core Business Domain

The application models a floral arrangement business with these key entities:

**Order Flow:**
- `Cliente` → places → `Pedido` → contains → `Arreglo`
- `Admin` manages and fulfills orders
- Orders can have optional on-site service (`Servicio`) with event details

**Inventory System:**
- `Inventario`: Raw materials/components (flowers, ribbons, etc.)
- `ArregloInventario`: Many-to-many relationship defining what inventory items and quantities each arrangement needs
- `PedidoInventario`: Additional inventory items added directly to a specific order

**State Management:**
- `Pedido.EstadoPedido`: Pendiente → EnProceso → Entregado/Cancelado
- `Arreglo.Disponible`: Computed property checking seasonal availability and inventory sufficiency

### Key Architectural Patterns

**Factory Pattern:**
- `ArregloFabrica` creates pre-configured arrangements based on `TipoArregloEnum`
- Sets default prices, time estimates, and occasions for standard arrangements

**Service Layer:**
- `ServicioPedido` encapsulates order creation business logic
- Validates inventory availability, calculates totals, applies business rules (e.g., orders >$5000 require manager authorization)
- Uses `SolicitudPedido` DTO for order creation

**Seasonal Logic:**
- `Arreglo.TemporadaArreglo` restricts availability based on current month
- `ObtenerTemporadaActual()` method maps months to seasons (Primavera/Verano/Otonio/Invierno)

**Enumerations Define Business Rules:**
- `EstadoPedido`, `ModoPago`, `TipoArregloEnum`, `OcasionArreglo`, etc.
- All enums are strongly typed (not strings)

### Data Layer

**DbContext:** `ApplicationDbContext` (in `Data/ApplicationDbContext.cs`)
- Inherits from `IdentityDbContext` for ASP.NET Identity integration
- Exposes all entity `DbSet<T>` properties

**Entity Relationships:**
- `Cliente` 1:N `Pedido` (required)
- `Admin` 1:N `Pedido` (optional - assigned after creation)
- `Arreglo` 1:N `Pedido` (required)
- `Arreglo` N:M `Inventario` (via `ArregloInventario`)
- `Pedido` N:M `Inventario` (via `PedidoInventario`)

**Important Entity Features:**
- `[NotMapped]` computed properties: `Cliente.PedidosActuales`, `Admin.HistorialPedidos`, `Arreglo.Disponible`, etc.
- Snapshot pattern: `Pedido.NombreCliente` stores customer name at order time (denormalized)

### Controllers

Most controllers follow standard scaffolded CRUD pattern with EF Core:
- `ClientesController`, `AdminsController`, `PedidosController`, etc.
- Include related entities using `.Include()` for navigation properties
- Use `SelectList` for dropdowns in Create/Edit views

**Special Controllers:**
- `HomeController`: Landing page
- `CatalogoController`: Public catalog view
- `ReportesController`: Business reporting

### Views Organization

**Shared Views:** `Views/Shared/`
- `_Layout.cshtml`: Main layout template
- `_LoginPartial.cshtml`: Identity login/logout UI
- `Error.cshtml`: Error page

**CRUD Views:** `Views/<Entity>/`
- Standard: Index, Create, Edit, Delete, Details

**Custom Admin Views:** `Views/VistaAdmin/`
- Dashboard, Entregas, Inventario, Reportes, Clientes

**Custom User Views:** `Views/VistaUsuario/`
- Inicio, MisPedidos, NuevoPedido, Historial, Notificaciones

**Static Assets:** `wwwroot/`
- `css/`, `js/`, `images/`: Custom assets
- `lib/`: Client-side libraries (Bootstrap, jQuery, validation)
- Organized by view context: `VistaAdmin/`, `VistaUsuario/`

### Models Organization

All models in `Models/` namespace, key files:
- **Entities:** `Cliente.cs`, `Admin.cs`, `Pedido.cs`, `Arreglo.cs`, `Inventario.cs`
- **Junction Tables:** `ArregloInventario.cs`, `PedidoInventario.cs`
- **DTOs:** `Solicitudes.cs` (SolicitudPedido, SolicitudInicioSesion, etc.)
- **Services:** `ServicioPedidos.cs`
- **Factories:** `ArregloFabrica.cs`

## Development Guidelines

### Authentication & Authorization
- Project uses ASP.NET Identity with `IdentityUser`
- User secrets configured (ID: `aspnet-Fiora-516f7f8a-d628-4ab7-9f73-6a49394b6ea5`)
- Email confirmation required by default (`RequireConfirmedAccount = true`)
- Separate `Cliente` and `Admin` entities exist alongside Identity (custom authentication logic)

### Database Connection
- Uses LocalDB: `Server=(localdb)\\mssqllocaldb`
- Database name: `aspnet-Fiora-516f7f8a-d628-4ab7-9f73-6a49394b6ea5`
- Connection string in `appsettings.json`

### Entity Framework Conventions
- Always use `.Include()` for navigation properties in queries
- Use `async`/`await` pattern for all database operations
- Check `ModelState.IsValid` before saving entities
- Handle `DbUpdateConcurrencyException` in Edit operations

### Business Rule Implementation
- Inventory validation happens in `ServicioPedido.ValidarPedido()`
- Order totals calculated from arrangement price + additional inventory items
- Seasonal availability checked via `Arreglo.Disponible` computed property
- Large orders (>$5000) flagged for manager authorization

### View Conventions
- Views use Spanish labels and messages
- Client-side validation via jQuery Validation
- Use `ViewData` for passing SelectLists to forms
- AntiForgeryToken required on all POST actions

### When Modifying Entities
1. Update the model class in `Models/`
2. Add migration: `dotnet ef migrations add <DescriptiveName>`
3. Review generated migration for correctness
4. Apply migration: `dotnet ef database update`
5. Update related controllers if navigation properties changed
6. Regenerate scaffolded views if needed

### When Adding New Features
- Follow existing MVC patterns (controller actions, view models, Razor views)
- Add business logic to service classes (like `ServicioPedido`), not controllers
- Use enums for fixed domain values rather than magic strings
- Consider seasonal/availability logic for inventory-related features
