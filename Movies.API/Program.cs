using Microsoft.EntityFrameworkCore;
using Movies.API.Extensions;
using Movies.Contracts;
using Movies.Core.Domain.Contracts;
using Movies.Data;
using Movies.Data.Repositories;
using Movies.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();
//AddScoped is good to use here, because that will create one instance that is used throughout the http request,
// To compare, Singleton would not be good because it creates an instance that is used for the entire application lifetime
//which leads to shared state between all users and safety issues with concurrent requests,
//and Transient that creates a new instance every time the service is requested
//can lead to performance overhead from constant object creation and object relationships within a request might get broken

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.SeedDataAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
