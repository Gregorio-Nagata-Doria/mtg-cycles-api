using Ciclopedia.Api.Repositories;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<AnosRepository>();
builder.Services.AddScoped<SetsRepository>();
builder.Services.AddScoped<CiclosRepository>();
builder.Services.AddProblemDetails();




builder.Services.AddControllers(options =>
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("Ciclopedia")!);
var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();
