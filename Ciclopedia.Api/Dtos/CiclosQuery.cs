using System.ComponentModel.DataAnnotations;

namespace Ciclopedia.Api.Dtos
{

    

    public record CiclosQuery

    (
        string[] Set,
        string[] Rarity,
        string[] Structure,
        string[] Color,
        int[] Year,
        string? Q,
         string? Sort,
        int Page = 1,
        int PerPage = 24

        ) : IValidatableObject
    {
        static readonly string[] validRarities = { "common", "uncommon", "rare", "mythic" };
        static readonly string[] validStructure = { "5-mono", "10-duplas", "5-aliadas", "5-inimigas", "artefatos", "terras" };
        static readonly string[] validColors = { "w", "u", "b", "r", "g", "c" };

        public IEnumerable<ValidationResult> Validate(ValidationContext contexto)
        {


            if (Page < 1)
            {
                yield return new ValidationResult("Deve ser 1 ou maior.", new[] { nameof(Page) });
            }
            if (PerPage is not < 101 || PerPage is not > 0)
            {
                yield return new ValidationResult("Deve estar entre 1 e 100.", new[] { nameof(PerPage) });
            }
            if (Rarity is not null && !Rarity.All(validRarities.Contains))
            {
                yield return new ValidationResult("Valores aceitos: common, uncommon, rare, mythic.", new[] { nameof(Rarity) });
            }
            if (Structure is not null && !Structure.All(validStructure.Contains))
            {
                yield return new ValidationResult("Valores aceitos: 5-mono, 10-duplas, 5-aliadas, 5-inimigas, artefatos, terras.", new[] { nameof(Structure) });
            }
            if (Color is not null && !Color.All(validColors.Contains))
            {
                yield return new ValidationResult("Valores aceitos: w, u, b, r, g, c (minúsculas).", new[] { nameof(Color) });
            }
            if (Sort is not (null or "" or "name-asc" or "name-desc" or "year-asc" or "year-desc"))

            {
                yield return new ValidationResult("Valores aceitos: name-asc, name-desc, year-asc, year-desc, ou vazio.", new[] { nameof(Sort) });
            }

        }
    
    };


}
