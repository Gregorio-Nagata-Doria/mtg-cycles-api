namespace Ciclopedia.Api.Dtos
{
    public record CicloResumoDto
    (
        string Slug,
        NomeDto Nome,
        string? SetCode,
        string? SetName,
        int? Year,
        string? Rarity,
        string? Structure,
        string Colors,
        IReadOnlyList<string> Thumbs
        )
        ;
    

}
