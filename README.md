# ASP.NET Core API Performance Optimization with Sentry

This repository contains an ASP.NET Core API for a Todo application that uses Sentry for performance monitoring and distributed tracing.

## Features

- CRUD operations for Todo items with parent-child relationships
- PostgreSQL database integration with Entity Framework Core
- Sentry instrumentation for performance monitoring and error tracking
- Distributed tracing for API requests
- Custom middleware for tracking request timing

## Prerequisites

- A valid Sentry DSN, otherwise the application won't start
- Docker

## Configure Sentry

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


## Run the Application

Ensure you have set your DSN, as shown in the previous section.

The safest, fastest, and cleanest way to run this application is with Docker, as shown in the commands below. This will work on any operating system.

> If you already have .NET installed on your machine, you can run the .NET commands locally instead, but you will need to set `Host=localhost` in the `.env` file.

Open a terminal and start Postgresql by running:

```sh
docker network create sentryNetwork;
docker run --init -it --platform=linux/amd64 --rm --name postgres --network sentryNetwork -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=password -e POSTGRES_DB=todos -p 5432:5432  postgres:17.5;
```

In another terminal start .NET by running:
```sh
docker run --init  -it --platform=linux/amd64 --rm --name app --network sentryNetwork  -v ".:/app" -w "/app" -p 5236:5236 mcr.microsoft.com/dotnet/sdk:9.0 bash;

# Inside the container now:
dotnet tool install --global dotnet-ef;
export PATH="$PATH:/root/.dotnet/tools";
dotnet ef database update;
```

In another new terminal that you can close afterwards, populate the database by running:

```sh
docker exec -i postgres psql -U admin -d todos < seed.sql
```

Back in the second terminal, start the app by running:
```sh
dotnet run;
```

Browse to the app at http://localhost:5236/api/todo

## API Endpoints

- `GET /api/todo` - Get all todo items
- `GET /api/todo/{id}` - Get a specific todo item
- `POST /api/todo` - Create a new todo item
- `PUT /api/todo/{id}` - Update a todo item
- `DELETE /api/todo/{id}` - Delete a todo item

## Performance Monitoring Features

- **Request Timing Middleware**: Custom middleware to track request timing statistics
- **Sentry Distributed Tracing**: Automatically captures distributed traces across API calls
- **Database Performance Tracking**: Monitors database query performance
- **Entity Framework Core Performance**: Optimized EF Core queries with eager loading and caching
