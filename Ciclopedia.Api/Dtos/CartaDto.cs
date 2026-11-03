namespace Ciclopedia.Api.Dtos
{


    public record CartaDto(
        NomeDto Nome,
        NomeDto TypeLine,
        string Image,
        string ScryfallId,
        string Set,
        string CollectorNumber,
        string Rarity,
        int Cmc,
        DateOnly ReleasedAt,
        IReadOnlyList<string> Cores
        );

}
