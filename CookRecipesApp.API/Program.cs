using CookRecipesApp.API.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Načtení connection stringu z appsettings.json (nebo User Secrets)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrace DbContextu
builder.Services.AddDbContext<CookRecipesDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();


