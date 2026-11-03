
using Dapper;
using Npgsql;
namespace Ciclopedia.Api.Repositories
{

    public class AnosRepository
    {

        private readonly NpgsqlDataSource _fonte;

        public AnosRepository(NpgsqlDataSource fonte) => _fonte = fonte;




        public async Task<IEnumerable<int>> ListarAnos()
        {



            await using var conn = await _fonte.OpenConnectionAsync();

            return await conn.QueryAsync<int>("SELECT DISTINCT year FROM ciclos WHERE year IS NOT NULL ORDER BY year DESC");

        }
    }
}
