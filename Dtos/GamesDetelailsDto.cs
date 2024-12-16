namespace GameStore.Dtos
{
    public record class GamesDetelailsDto
        (int Id,
        string Name,
        int GenreId,
        decimal Price,
        DateOnly ReleseDate);
}
