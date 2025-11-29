# Multi-Tenant Portal Application

## Project Overview

A multi-tenant portal application built with ABP Framework that allows multiple organizations to share the same application instance while maintaining data isolation. The system consists of three main portals:

- **Host Portal** - For system administrators to manage tenants
- **Internal Portal** - For tenant staff members to manage users and announcements
- **External Portal** - Public-facing portal for tenant customers

## Key Features

- **Multi-Tenancy** with shared database approach
- **Role-based access control** (Staff vs Public Users)
- **Tenant isolation** using TenantId in shared database
- **Custom user types** (Public/Staff) via extended IdentityUser
- **Announcement management** with publishing controls
- **Separate layouts** for each portal type

## Database Architecture

### Shared Database Approach

This implementation uses a **shared database** with tenant isolation through `TenantId` columns, instead of separate databases per tenant. This approach provides:

- **Simplified deployment** and maintenance
- **Better resource utilization**
- **Easier cross-tenant reporting** (when required)
- **Reduced operational overhead**

### Required Entities

#### Tenant (Extended ABP Tenant)
- `Name` (ABP built-in)
- `DisplayName` (ABP built-in)
- `Country` (string) - *Custom field*
- `ContactEmail` (string) - *Custom field*

#### User (Extended ABP IdentityUser)
- All standard IdentityUser properties
- `UserType` (enum: Public, Staff) - *Custom field*

#### Announcement
- `Title` (string)
- `Content` (string)
- `IsPublished` (boolean)
- `PublishDate` (datetime)
- `TenantId` (Guid) - For tenant isolation

## Portal Structure

### Host Portal
- **Tenant List Page** - View and manage all tenants
- **Tenant Edit Page** - Modify tenant information

### Internal Portal (Staff)
- **Dashboard** - Overview of users and announcements
- **Public User Management** - CRUD operations for public users
- **Announcement Management** - Create and manage announcements

### External Portal (Public)
- **Home Page** - Display published announcements
- **Registration Page** - Public user registration

## Implementation Details

### Multi-Tenancy Configuration

The application uses ABP's built-in multi-tenancy features with these customizations:

```csharp
// In your module configuration
Configure<AbpMultiTenancyOptions>(options =>
{
    options.IsEnabled = true;
});

// Database configuration for shared database
Configure<AbpDbConnectionOptions>(options =>
{
    options.ConnectionStrings.Default = "YourSharedConnectionString";
});
```

### Tenant Resolution

- **Production**: Tenant resolved from URL (e.g., `tenant1.domain.com`)
- **Development**: Tenant can be overridden via cookies for testing

### Data Isolation

All tenant-specific entities implement `IMustHaveTenant` interface:

```csharp
public class Announcement : AggregateRoot<Guid>, IMustHaveTenant
{
    public Guid TenantId { get; set; }
    // Other properties...
}
```

## Getting Started

### Prerequisites

- .NET SDK 10.0+
- ABP CLI
- SQL Server
- Visual Studio 2022+

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd MultiTenantPortal
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection**
   - Modify `appsettings.json` with your SQL Server connection string

4. **Run database migrations**
   ```bash
   # Using the DBMigrator project (recommended)
   cd src/unimade.MTPortal.DbMigrator
   dotnet run
   ```
   
   *Alternatively, you can use EF Core commands:*
   ```bash
   dotnet ef database update
   ```

5. **Set the Web project as startup project**
   - In Visual Studio: Right-click on `unimade.MTPortal.Web` project → "Set as Startup Project"
   - Or using CLI:
   ```bash
   cd src/unimade.MTPortal.Web
   dotnet run
   ```

6. **Run the application**
   - Press F5 in Visual Studio, or
   ```bash
   dotnet run --project src/unimade.MTPortal.Web
   ```
   - The application will be available at `https://localhost:44344`

## Security & Access Control

- **Host Portal**: Accessible only to host administrators
- **Internal Portal**: Restricted to staff users of each tenant
- **External Portal**: Public access with user registration
- **Data Isolation**: Automatic tenant filtering using ABP's data filters

## Best Practices Implemented

- **Domain-Driven Design** principles
- **Repository pattern** with ABP's generic repositories
- **Dependency Injection** throughout the application
- **Clean separation** of concerns between portals
- **Proper multi-tenant** data isolation in shared database
- **Extensible entity design** for future enhancements

## Deployment Notes

The shared database approach simplifies deployment by requiring only one database instance. Ensure:

- Proper indexing on `TenantId` columns for performance
- Regular database backups
- Monitoring of database growth
- Tenant data isolation validation in queries

## Support

For technical support or questions about this implementation, please contact the development team.

---

*This project demonstrates advanced ABP Framework capabilities including multi-tenancy, entity extension, and portal separation using a shared database architecture.*