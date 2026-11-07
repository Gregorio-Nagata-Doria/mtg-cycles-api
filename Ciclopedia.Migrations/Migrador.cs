using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DbUp;
using DbUp.Engine;

namespace Ciclopedia.Migrations
{
    public class Migrador
    {


        public static UpgradeEngine CriarMotor(string connectionString)
        {

            var motor = DeployChanges.To
    .PostgresqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();

            return motor;

        }
    }
}
