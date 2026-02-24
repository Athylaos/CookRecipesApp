using CookRecipesApp.API.Context;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CookRecipesApp.API.Endpoints
{
    public static class IngredientEndpoint
    {
        public static void MapIngredientEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/ingredients");


            group.MapGet("/getPreviews", async (int? amount, CookRecipesDbContext db) =>
            {
                var query = db.Ingredients
                    .AsNoTracking()
                    .Select(i => new IngredientPreview
                    {
                        Id = i.Id,
                        Name = i.Name,
                        DefaultUnit = i.DefaultUnitNavigation,

                        IngredientUnits = i.IngredientUnits.Select(iu => new UnitPreviewDto
                        {
                            Id = iu.UnitId,
                            Name = iu.Unit.Name,
                            ConversionFactor = iu.ToDefaultUnit
                        }).ToList()
                    });

                if (amount.HasValue && amount > 0)
                {
                    query = query.Take(amount.Value);
                }

                return Results.Ok(await query.ToListAsync());
            });

        }

    }
}
