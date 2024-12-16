using GameStore.Data;
using GameStore.Mapping;
using System.Data.Entity;
using System.Threading.Tasks.Dataflow;

namespace GameStore.EndPoints
{
    public static class GenreEndpoints
    {
        public static WebApplication MapGenresEndpoints(this WebApplication app)
        {
            app.MapGet("genres", async (GameStoreContext dbContext) =>
                await dbContext.Genres
                    .Select(genre => genre.ToDto())
                    .AsNoTracking()
                    .ToListAsync());

            return app;
        }
    }
}
