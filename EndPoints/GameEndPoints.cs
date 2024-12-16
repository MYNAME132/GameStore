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

            // POST: Add a new game
            group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                if (!dbContext.Genres.Any(g => g.Id == newGame.GenreId))
                {
                    return Results.BadRequest("Invalid GenreId.");
                }

                Game game = newGame.ToEntity();
                dbContext.Games.Add(game);
                dbContext.SaveChanges();

                dbContext.Entry(game).Reference(g => g.Genre).Load();

                return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game.ToGameDetailsDto());
            });

            // PUT: Update a game by ID
            group.MapPut("/{id}", (int id, UpdateGameDto updatedGame,GameStoreContext dbContext) =>
            {
                var existingGame=dbContext.Games.Find(id);
                if (existingGame == null)
                {
                    return Results.NotFound();
                }

                existingGame.Name = updatedGame.Name;
                existingGame.GenreId = updatedGame.GenreId;
                existingGame.Price = updatedGame.Price;
                existingGame.Date = updatedGame.ReleseDate;

                dbContext.SaveChanges();

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
