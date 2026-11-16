using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Npgsql;

namespace Ciclopedia.Api.Tests.Integracao
{

    [Collection("Banco")]
    public class BancoTesteFixtureTests
    {


        private readonly BancoTesteFixture _fixture;

        public BancoTesteFixtureTests(BancoTesteFixture fixture) => _fixture = fixture;


        [Fact]

        public async Task ContaCiclos()
        {
            await using var conn = new NpgsqlConnection(_fixture.ConnectionString);
            await conn.OpenAsync();
            var total = await conn.QuerySingleAsync<long>("SELECT count(*) FROM ciclos");
            Assert.Equal(0, total);
        }

    }
}
