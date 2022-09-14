using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

EvolveMigrator.MigrateDatabase(assemblyName: builder.Environment.ApplicationName,
    connectionString: "Server=localhost;Port=5432;Database=test;User Id=postgres;Password=postgres;",
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
