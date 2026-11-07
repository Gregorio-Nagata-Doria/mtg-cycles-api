using System.Reflection;
using Ciclopedia.Migrations;
using DbUp;


var cs = Environment.GetEnvironmentVariable("ConnectionStrings__Ciclopedia");

var motor = Migrador.CriarMotor(cs);

var resultado = args.Contains("--marcar") ? motor.MarkAsExecuted() : motor.PerformUpgrade();
return resultado.Successful ? 0 : 1;