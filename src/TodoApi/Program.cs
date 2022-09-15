using System.Data;
using Npgsql;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("EchoApi")
    .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri("http://localhost:5500"));

builder.Services.AddTransient<IDbConnection>((services) =>
 new NpgsqlConnection(
        services.GetRequiredService<IConfiguration>()
            .GetConnectionString("TestDb"))
);

var app = builder.Build();

EvolveMigrator.MigrateDatabase(assemblyName: builder.Environment.ApplicationName,
    connectionString: app.Configuration.GetConnectionString("TestDb"),
    defaultDbName: "postgres",
    includeSeedData: true,
    dropDatabase: true,
    Console.WriteLine); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}