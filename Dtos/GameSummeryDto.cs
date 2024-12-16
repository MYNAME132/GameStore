namespace GameStore.Dtos
{
    public record class GameSummeryDto(
        int Id,
        string Name,
        string Genre,
        decimal Price,
        DateOnly ReleseDate);
}
