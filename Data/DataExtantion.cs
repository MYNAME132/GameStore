using Microsoft.EntityFrameworkCore;

namespace GameStore.Data
{
    public static class WebApplicationExtensions
    {
        public static async Task MigrateDbAsync(this WebApplication app)
        {
            // Create a scope to resolve the required services
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

            // Apply any pending migrations
            await dbContext.Database.MigrateAsync();
        }
    }
}