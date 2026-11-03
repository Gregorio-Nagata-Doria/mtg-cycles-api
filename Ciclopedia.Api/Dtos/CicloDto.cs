namespace Ciclopedia.Api.Dtos
{


    public record CicloDto(
        string Slug,
        IReadOnlyList<CartaDto> Cartas,
        NomeDto Nome,
        string? SetCode,
        string? SetName,
        int? Year,
        string? Rarity,
        string? Structure,
        string Colors
        );

    public record NomeDto(
        string? Pt, string En);
}
