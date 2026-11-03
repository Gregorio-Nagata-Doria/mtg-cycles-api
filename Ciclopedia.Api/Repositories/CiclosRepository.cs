using Dapper;
using Npgsql;
using Ciclopedia.Api.Dtos;
using Ciclopedia.Api.Helpers;
using System.Reflection.Metadata.Ecma335;
namespace Ciclopedia.Api.Repositories

{

    public class CiclosRepository
    {

        private readonly NpgsqlDataSource _fonte;

        public CiclosRepository(NpgsqlDataSource fonte) => _fonte = fonte;

        private record CicloLinha(
            string Slug,
            string NomePt,
            string NomeEn,
            string? Set,
            int? Year,
            string? Rarity,
            string? Structure,
            string Colors,
            string? SetName
            );

        private record CartaLinha
        {
            public string? NomePt { get; init; }
            public string NomeEn { get; init; } = "";
            public string? TypeLinePt { get; init; }
            public string TypeLine { get; init; } = "";
            public string Image { get; init; } = "";
            public string ScryfallId { get; init; } = "";
            public string Set { get; init; } = "";
            public string CollectorNumber { get; init; } = "";
            public string Rarity { get; init; } = "";
            public int Cmc { get; init; }
            public DateOnly ReleasedAt { get; init; }
            public string[]? Cores { get; init; }
        }


        private record CicloResumoLinha
        {
            public string Slug { get; init; } = "";
            public string NomePt { get; init; } = "";
            public string NomeEn { get; init; } = "";
            public string? Set { get; init; }
            public int? Year { get; init; }
            public string? Rarity { get; init; }
            public string? Structure { get; init; }
            public string Colors { get; init; } = "";
            public string? SetName { get; init; }
            public string[]? Thumbs { get; init; }
        }


        public async Task<CicloDto?> BuscarCiclo(string slug)
        {
            var slugInterno = SlugHelper.Interno(slug);

            if (slugInterno is null) return null;


            await using var conn = await _fonte.OpenConnectionAsync();

     

            var l = await conn.QuerySingleOrDefaultAsync<CicloLinha>("SELECT c.slug, " +
                "c.nome_pt AS NomePt, c.nome_en AS NomeEn, c.set," +
                " c.year, c.rarity, c.structure," +
                " c.colors, s.setname FROM ciclos c " +
                "LEFT JOIN sets s ON c.set = s.setCode WHERE c.slug = @slug", ( new {slug= slugInterno }));


            if (l is null) return null;


            var linhasCartas = await conn.QueryAsync<CartaLinha>("SELECT ct.nome_pt AS NomePt, ct.nome_en AS NomeEn," +
                " ct.typelinept, ct.typeline, ct.image, ct.scryfallid,\r\n  " +
                "     ct.set, ct.collectornumber, ct.rarity, ct.cmc, ct.releasedat,\r\n   " +
                "    (SELECT array_agg(cc.cor ORDER BY " +
                "array_position(ARRAY['W','U','B','R','G'], cc.cor))\r\n    " +
                "    FROM carta_cor cc WHERE cc.carta_id = ct.id) AS cores\r\nFROM cartas ct\r\n" +
                "WHERE ct.ciclo = @slug\r\nORDER BY ct.posicao", (new {slug = slugInterno }));


            var cartas = linhasCartas.Select(x => new CartaDto(
                Nome: new NomeDto(
                    x.NomePt,
                    x.NomeEn),
                TypeLine: new NomeDto(
                    x.TypeLinePt,
                   x.TypeLine),
                Image: x.Image,
                ScryfallId: x.ScryfallId,
                Set: x.Set,
                CollectorNumber: x.CollectorNumber,
                Rarity: x.Rarity,
                Cmc: x.Cmc,
                ReleasedAt: x.ReleasedAt,
                Cores: x.Cores is null ? Array.Empty<string>() : x.Cores
            )).ToList();

                return new CicloDto(SlugHelper.Publico(slugInterno), cartas,
                  new NomeDto(l.NomePt, l.NomeEn)
                    , l.Set, l.SetName, l.Year, l.Rarity, l.Structure, l.Colors);
        }




        public async Task<(IReadOnlyList<CicloResumoDto> Dados, long Total)> ListarCiclos(CiclosQuery filtro)
        {
            await using var conn = await _fonte.OpenConnectionAsync();

            var p = new DynamicParameters();
            var cond = new List<string>();

            if (filtro.Set is { Length: > 0 })
            {
                cond.Add("set = ANY(@set)");
                p.Add("set", filtro.Set);
             
            }
            if (filtro.Rarity is { Length: > 0 })
            {
                cond.Add("rarity = ANY(@rarity)");
                p.Add("rarity", filtro.Rarity);
            
            }
            if (filtro.Structure is { Length: > 0 })
            {
                cond.Add("c.structure = ANY(@structure)");
                p.Add("structure", filtro.Structure);
               
            }
            if (filtro.Year is { Length: > 0 })
            {
                cond.Add("year = ANY(@year)");
                p.Add("year", filtro.Year);
            
            }
            if (filtro.Color is { Length: > 0 })
            {


                string[] colorFilter = filtro.Color.Where(x => x != "c").Select(x => x.ToUpper()).ToArray();

                bool colorless = filtro.Color.Contains("c");

                cond.Add("(string_to_array(c.colors, NULL) && @cores OR (@incolor AND c.colors = ''))");

                p.Add("cores", colorFilter);
                p.Add("incolor", colorless);
            

            }
            if (!string.IsNullOrWhiteSpace(filtro.Q))
            {
                string[] termos = filtro.Q.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Replace(@"\", @"\\").Replace("%",@"\%").Replace("_",@"\_")).ToArray();
                cond.Add("unaccent(lower(c.nome_pt || ' ' || c.nome_en || ' ' || coalesce(s.setname, '')))\r\n  LIKE ALL (    SELECT '%' || unaccent(lower(termo)) || '%'\r\n    FROM unnest(@termos) AS t(termo))");
                p.Add("termos", termos);
             

            }

            var where = cond.Count > 0
? " WHERE " + string.Join(" AND ", cond)
: "";


            string ordenacao = filtro.Sort switch
            {
                "name-asc" => " ORDER BY nome_pt, slug",
                "name-desc" => " ORDER BY nome_pt DESC, slug DESC",
                "year-asc" => " ORDER BY year ASC NULLS LAST, slug",
                "year-desc" => " ORDER BY year DESC NULLS LAST, slug",
                _ => " ORDER BY slug",


            };


            p.Add("perPage", filtro.PerPage);
            p.Add("offset", filtro.PerPage * (filtro.Page - 1));


            var sql = "SELECT count(*) FROM ciclos c LEFT JOIN sets s ON c.set = s.setCode"
                + where + ";" +
                "SELECT  c.slug, c.nome_pt AS NomePt, c.nome_en AS NomeEn, c.set, c.year, " +
                "c.rarity, c.structure, c.colors, s.setname, " +
                "(SELECT array_agg(ct.scryfallid ORDER BY ct.posicao)" +
                " FROM cartas ct WHERE ct.ciclo = c.slug) AS thumbs" +
                " FROM ciclos c LEFT JOIN sets s ON c.set = s.setCode"
                + where + ordenacao + " LIMIT @perPage OFFSET @offset"
                ;

            await using var grade = await conn.QueryMultipleAsync(sql, p);
            var total = await grade.ReadSingleAsync<long>();
            var resumo = await grade.ReadAsync<CicloResumoLinha>();

            var dados = resumo.Select(x => new CicloResumoDto(
          Slug: SlugHelper.Publico(x.Slug),
 Nome: new NomeDto(x.NomePt, x.NomeEn),
SetCode: x.Set,
SetName: x.SetName,
Year: x.Year,
  Rarity: x.Rarity,
    Structure: x.Structure,
   Colors: x.Colors,
   Thumbs: x.Thumbs is null ? Array.Empty<string>() : x.Thumbs.Select(id => UrlHelper.Miniatura(id)).ToArray()
   )).ToList();
            return (dados, total);
        }

     

    }



    }
