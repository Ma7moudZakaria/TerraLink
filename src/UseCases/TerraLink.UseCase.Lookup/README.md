# TerraLink.UseCase.Lookups

This UseCase module provides API endpoints for managing Lookup Sets and Lookup Items in the TerraLink application.

## Overview

Lookups are used tEmployeeoughout the application to manage reference data such as governorates, cities, districts, building statuses, unit types, finishing types, and more.

## Architecture

The module follows Clean Architecture principles with the following layers:

### Applications Layer
- **Interfaces**: Defines contracts for repositories and services
- **Features/Endpoints**: Contains minimal API endpoints

### Infrastructure Layer
- **Repositories**: Data access implementations
- **Services**: Business logic implementations

## Endpoints

### 1. Get All Lookup Sets
- **Route**: `GET /api/lookups/sets`
- **Description**: Retrieves all active lookup sets with their items
- **Response**: `List<LookupSetEntity>`

### 2. Get Lookup Items By Set Id
- **Route**: `GET /api/lookups/sets/{LookupSetId}/items`
- **Description**: Retrieves all active lookup items for a specific lookup set
- **Parameters**: 
  - `LookupSetId` (Guid) - The ID of the lookup set
- **Response**: `List<LookupItemEntity>`

### 3. Add Lookup Set
- **Route**: `POST /api/lookups/sets`
- **Description**: Creates a new lookup set
- **Request Body**:
  ```json
  {
    "code": "string",
    "descriptions": {
  "en": "English Description",
      "ar": "Arabic Description"
    }
  }
  ```
- **Response**: `Guid` (ID of the created lookup set)

### 4. Add Lookup Item
- **Route**: `POST /api/lookups/items`
- **Description**: Creates a new lookup item for a specific lookup set
- **Request Body**:
  ```json
  {
  "lookupSetId": "guid",
    "code": "string",
    "descriptions": {
      "en": "English Description",
      "ar": "Arabic Description"
    },
    "sortOrder": 0,
    "metadata": {
      "key1": "value1",
      "key2": "value2"
    }
  }
  ```
- **Response**: `Guid` (ID of the created lookup item)

## Services

### ILookupSetService
Handles business logic for lookup sets:
- `GetAllAsync()`: Retrieves all active lookup sets
- `AddAsync()`: Creates a new lookup set with validation

### ILookupItemService
Handles business logic for lookup items:
- `GetBySetIdAsync()`: Retrieves items for a specific set
- `AddAsync()`: Creates a new lookup item with validation

## Repositories

### ILookupSetRepository
Data access for lookup sets:
- Includes related lookup items
- Orders by code
- Filters active records

### ILookupItemRepository
Data access for lookup items:
- Filters by lookup set ID
- Orders by sort order and code
- Filters active records

## Dependencies

This module depends on:
- `TerraLink.Persistence` - For database context and entity configurations
- `LowCodeHub.QueryableExtensions` - For base entities and value objects
- `LowCodeHub.MinimalEndpoints` - For minimal API abstractions
- `ErrorOr` - For functional error handling

## Registration

To register this module in your application:

```csharp
// In Program.cs or Startup.cs
services.AddLookupsUseCase();

// Add routes
app.MapModule<LookupsModule>();
```

## OpenAPI Documentation

The module provides its own OpenAPI documentation group accessible at:
- Swagger UI: `/swagger/lookup-module/swagger.json`

## Usage Examples

### Creating a Lookup Set
```bash
POST /api/lookups/sets
Content-Type: application/json

{
  "code": "GOVERNORATES",
"descriptions": {
    "en": "Governorates",
"ar": "?????????"
  }
}
```

### Creating a Lookup Item
```bash
POST /api/lookups/items
Content-Type: application/json

{
  "lookupSetId": "your-lookup-set-id",
  "code": "CAIRO",
  "descriptions": {
    "en": "Cairo",
    "ar": "???????"
  },
  "sortOrder": 1,
  "metadata": {
    "region": "central"
  }
}
```

### Fetching All Lookup Sets
```bash
GET /api/lookups/sets
```

### Fetching Items for a Specific Set
```bash
GET /api/lookups/sets/{lookupSetId}/items
```

## Features

- ? CRUD operations for lookup sets
- ? CRUD operations for lookup items
- ? Localized descriptions support (English/Arabic)
- ? Metadata support for extensibility
- ? Sort order management
- ? Active/Inactive filtering
- ? Validation and error handling
- ? OpenAPI/Swagger documentation
- ? Clean architecture separation
- ? Dependency injection ready

## Future Enhancements

Potential improvements:
- Update endpoints for sets and items
- Delete/Deactivate endpoints
- Bulk operations
- Search and filtering
- Pagination support
- Caching layer
- Authorization/permissions
