using GameStore.Data;
using GameStore.Dtos;
using GameStore.EndPoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.
var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

// Build the application after configuring services.
var app = builder.Build();

// Apply migrations at runtime.
await app.MigrateDbAsync();

// Map endpoints.
app.MapGamesEndPoints();

app.MapGenresEndpoints();

// Run the application.
app.Run();