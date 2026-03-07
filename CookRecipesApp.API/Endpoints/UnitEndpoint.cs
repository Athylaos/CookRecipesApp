using CookRecipesApp.API.Context;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CookRecipesApp.API.Endpoints
{
    public static class UnitEndpoint
    {
        public static void MapUnitEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/units");

            //---------------------------------------------------------------Get servingUnits
            group.MapGet("/getServing", async (CookRecipesDbContext db) =>
            {
                var units = await db.Units.AsNoTracking().Where(u => u.IsServingUnit).Select(u => new UnitPreviewDto { Id = u.Id, Name = u.Name }).ToListAsync();

                return Results.Ok(units) ;
            });

            //---------------------------------------------------------------Get units
            group.MapGet("/get", async (CookRecipesDbContext db) =>
            {
                var units = await db.Units.AsNoTracking().Select(u => new UnitPreviewDto { Id = u.Id, Name = u.Name }).ToListAsync();

                return Results.Ok(units);
            });

            group.MapGet("/getRecipeUnits/{ingredientId:guid}", async (Guid ingredientId, ClaimsPrincipal user, CookRecipesDbContext db) =>
            {



            });
                

        }
    
    } 
}
