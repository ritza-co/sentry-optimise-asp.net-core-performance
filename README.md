# ASP.NET Core API Performance Optimization with Sentry

This repository contains an ASP.NET Core API for a Todo application that uses Sentry for performance monitoring and distributed tracing.

## Features

- CRUD operations for Todo items with parent-child relationships
- PostgreSQL database integration with Entity Framework Core
- Sentry instrumentation for performance monitoring and error tracking
- Distributed tracing for API requests
- Custom middleware for tracking request timing

## Prerequisites

- .NET 8.0 SDK or later
- PostgreSQL database
- A valid Sentry DSN, otherwise the application won't start

## Configuring Sentry

This application uses Sentry for performance monitoring, error tracking, and distributed tracing. Sentry configuration is managed in the `appsettings.json` file.

### Sentry Settings in appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "Sentry": {
      "Dsn": "<Sentry DSN value here>",
      "SendDefaultPii": true,
      "MaxRequestBodySize": "Always",
      "MinimumBreadcrumbLevel": "Debug",
      "MinimumEventLevel": "Warning",
      "AttachStackTrace": true,
      "Debug": true,
      "DiagnosticLevel": "Error",
      "TracesSampleRate": 1.0
    }
  },
  "AllowedHosts": "*"
}
```

### Key Sentry Configuration Parameters

- **Dsn**: Your Sentry project DSN (Data Source Name). You need to replace this with your own Sentry project DSN.
- **SendDefaultPii**: When true, Sentry will include personal identifiable information in error reports.
- **MaxRequestBodySize**: Controls how much of the request body is captured. Set to "Always" to capture the entire request body.
- **MinimumBreadcrumbLevel**: The minimum level of breadcrumb logging. Set to "Debug" for detailed tracking.
- **MinimumEventLevel**: The minimum level at which events are sent to Sentry. Set to "Warning" to avoid excessive event reporting.
- **AttachStackTrace**: When true, stack traces are attached to all events.
- **Debug**: Enables debug mode for Sentry SDK.
- **DiagnosticLevel**: The level at which diagnostic information is captured.
- **TracesSampleRate**: The sampling rate for performance traces (1.0 = 100% of requests).


### Install the .NET SDK

Install the [.NET SDK](https://dotnet.microsoft.com/download) for your platform.

### Install PostgreSQL

If you use macOS, install PostgreSQL using [Homebrew](https://brew.sh/):

```sh
brew install postgresql
```

For installation instructions on other operating systems, see the [PostgreSQL downloads page](https://www.postgresql.org/download/).

### Database Setup

1. Start PostgreSQL:

```bash
brew services start postgresql
```

2. Connect to PostgreSQL:

```bash
psql postgres
```

3. Create a user and database:

```sql
CREATE USER admin WITH PASSWORD '<your-password>';
CREATE DATABASE todos;
GRANT ALL PRIVILEGES ON DATABASE todos TO admin;
```

4. Exit the PostgreSQL prompt with `\q`.

5. Create a `.env` file in the root directory:

```
ConnectionStrings__DefaultConnection='Host=localhost;Database=todos;Username=admin;Password=<your-password>'
```

6. Run database migrations:

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

7. Seed the database (optional):

```bash
psql -d todos -f seed.sql
```

## Running the Application

1. Restore dependencies:

```bash
dotnet restore
```

2. Run the application:

```bash
dotnet run
```

3. The API will be available at:
   - <http://localhost:5000>
   - <https://localhost:5001> (if HTTPS is enabled)

## API Endpoints

- `GET /api/todos` - Get all todo items
- `GET /api/todos/{id}` - Get a specific todo item
- `POST /api/todos` - Create a new todo item
- `PUT /api/todos/{id}` - Update a todo item
- `DELETE /api/todos/{id}` - Delete a todo item

## Performance Monitoring Features

- **Request Timing Middleware**: Custom middleware to track request timing statistics
- **Sentry Distributed Tracing**: Automatically captures distributed traces across API calls
- **Database Performance Tracking**: Monitors database query performance
- **Entity Framework Core Performance**: Optimized EF Core queries with eager loading and caching
