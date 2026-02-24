using CookRecipesApp.API.Context;
using CookRecipesApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

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
                var units = await db.Units.AsNoTracking().Where(u => u.IsServingUnit).ToListAsync();

                return Results.Ok(units) ;
            });

            //---------------------------------------------------------------Get units
            group.MapGet("/get", async (CookRecipesDbContext db) =>
            {
                var units = await db.Units.AsNoTracking().ToListAsync();

                return Results.Ok(units);
            });

        }
    
    } 
}
