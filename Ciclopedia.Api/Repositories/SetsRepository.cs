using Dapper;
using Npgsql;
using Ciclopedia.Api.Dtos;
namespace Ciclopedia.Api.Repositories

{

    public class SetsRepository
    {

        private readonly NpgsqlDataSource _fonte;

        public SetsRepository(NpgsqlDataSource fonte) => _fonte = fonte;




        public async Task<IEnumerable<SetDto>> ListarSets()
        {



            await using var conn = await _fonte.OpenConnectionAsync();

            return await conn.QueryAsync<SetDto>("SELECT setcode as code, setname as name FROM sets ORDER BY setname");

        }
    }
}
