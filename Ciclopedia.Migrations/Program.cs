using System.Reflection;
using DbUp;


var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Ciclopedia");

var motor = DeployChanges.To
    .PostgresqlDatabase(cs)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();

var resultado = args.Contains("--marcar") ? motor.MarkAsExecuted() : motor.PerformUpgrade();
return resultado.Successful ? 0 : 1;