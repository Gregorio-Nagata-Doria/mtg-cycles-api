using System;
using System.Collections.Generic;
using System.Text;
using Ciclopedia.Migrations;
using DbUp;
using Npgsql;



namespace Ciclopedia.Api.Tests.Integracao
{
    public class BancoTesteFixture : IAsyncLifetime
    {
        public string ConnectionString { get; private set; } = "";
        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }

        ValueTask IAsyncLifetime.InitializeAsync()
        {
            var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Ciclopedia");

            if (cs == null)
            {
                throw new InvalidOperationException("Variável de ambiente 'ConnectionStrings__Ciclopedia' não encontrada.");
            }

            var montador = new NpgsqlConnectionStringBuilder(cs);
            
            montador.Database = "ciclopedia_test";
            ConnectionString = montador.ConnectionString;
            var newDatabase = new NpgsqlConnectionStringBuilder(ConnectionString).Database;

            if (!newDatabase.EndsWith("_test")) {
                throw new InvalidOperationException($"Recusado: o banco '{newDatabase}' não termina em '_test'.");
            }

            DropDatabase.For.PostgresqlDatabase(ConnectionString);
            EnsureDatabase.For.PostgresqlDatabase(ConnectionString);
            var resultado = Migrador.CriarMotor(ConnectionString).PerformUpgrade();

            if (!resultado.Successful)
            {
                throw new InvalidOperationException("Falha ao migrar o banco de teste.", resultado.Error);
            }
            return ValueTask.CompletedTask;

        }


        }
    }

