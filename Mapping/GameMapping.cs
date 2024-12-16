using GameStore.Dtos;
using GameStore.Entities;

namespace GameStore.Mapping
{
    public static class GameMapping
    {
        public static Game ToEntity(this CreateGameDto game)
        {
            return new Game()
            {
                Name = game.Name,
                GenreId = game.GenreId,
                Price = game.Price,
                Date = game.ReleseDate,
            };
        }

        public static GameSummeryDto ToGameSummeryDto(this Game game)
        {
            if (game.Genre == null)
            {
                throw new InvalidOperationException("Genre is not loaded.");
            }

            return new GameSummeryDto(
                game.Id,
                game.Name,
                game.Genre.Name, // Genre should be loaded at this point
                game.Price,
                game.Date
            );
        }
        public static GamesDetelailsDto ToGameDetailsDto(this Game game)
        {
            if (game.Genre == null)
            {
                throw new InvalidOperationException("Genre is not loaded.");
            }
            return new GamesDetelailsDto(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.Date
            );
        }

        public static Game ToEntity(this UpdateGameDto dto, int id)
        {
            return new Game
            {
                Id = id,         
                Name = dto.Name,
                GenreId = dto.GenreId,
                Price = dto.Price,
                Date = dto.ReleseDate
            };
        }
    }
}
