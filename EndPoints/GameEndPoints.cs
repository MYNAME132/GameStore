using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace GameStore.EndPoints
{
    public static class GameEndPoints
    {

        // Extension method to map game-related endpoints
        public static RouteGroupBuilder MapGamesEndPoints(this WebApplication app)
        {
            // Create a route group for "games" endpoints
            var group = app.MapGroup("games").WithParameterValidation();
            const string GetGameEndpointName = "GetGame";

            // GET: List all games
            group.MapGet("/", async (GameStoreContext dbContext) =>
            {
                var games = await dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameSummeryDto())
                    .ToListAsync();

                return Results.Ok(games);
            });

            // GET: Retrieve a game by ID
            group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);
                if (game == null)
                {
                    return Results.NotFound();
                }
                await dbContext.Entry(game).Reference(g => g.Genre).LoadAsync();
                return Results.Ok(game.ToGameDetailsDto());
            }).WithName(GetGameEndpointName);

            group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                if (!await dbContext.Genres.AnyAsync(g => g.Id == newGame.GenreId))
                {
                    return Results.BadRequest("Invalid GenreId.");
                }

                var game = newGame.ToEntity();
                await dbContext.Games.AddAsync(game);
                await dbContext.SaveChangesAsync();

                await dbContext.Entry(game).Reference(g => g.Genre).LoadAsync();

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
            });

            group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);
                if (existingGame == null)
                {
                    return Results.NotFound();
                }

                existingGame.Name = updatedGame.Name;
                existingGame.GenreId = updatedGame.GenreId;
                existingGame.Price = updatedGame.Price;
                existingGame.Date = updatedGame.ReleseDate;

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });

            // DELETE: Remove a game by ID
            group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);
                if (game == null)
                {
                    return Results.NotFound($"No game found with ID {id}.");
                }

                dbContext.Games.Remove(game);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });

            return group;
        }
    }
}
