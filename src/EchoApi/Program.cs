var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/echo", (string personName) => $"Echo !! > {personName}")
    .WithName("Echo");

app.MapGet("/hi", (string personName) => $"Hi, {personName}")
    .WithName("Hi");

app.Run();

public partial class Program
{
}