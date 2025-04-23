using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Middlewares;

DotNetEnv.Env.Load(); // Load .env file

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry(options =>
{
    options.Environment = builder.Environment.EnvironmentName;
    options.TracesSampleRate = 1.0;
    options.ProfilesSampleRate = 1.0;
});

builder
    .Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true
    )
    .AddEnvironmentVariables(); // Add environment variable support

// Read connection string from environment variables (.env loaded via DotNetEnv)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

// Configure EF Core to use PostgreSQL
builder.Services.AddDbContext<TodoContext>(options => options.UseNpgsql(connectionString));

// Add CORS service - Make sure this is before app.Build()
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowNextApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

var app = builder.Build();

// app.UseMiddleware<RequestTimingMiddleware>();

// Enable CORS - This MUST be early in the middleware pipeline
app.UseCors("AllowNextApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
