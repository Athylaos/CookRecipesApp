using Scalar.AspNetCore;
using CookRecipesApp.API.Context;
using Microsoft.EntityFrameworkCore;
using CookRecipesApp.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CookRecipesDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer((schema, context, cancellationToken) =>
    {
        return Task.CompletedTask;
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.MapUserEndpoints();




app.Run();


