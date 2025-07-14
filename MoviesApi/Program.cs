using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Extensions;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MoviesApiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MoviesApiContext") ?? throw new InvalidOperationException("Connection string 'MoviesApiContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.SeedDataAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
