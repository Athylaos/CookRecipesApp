using CookRecipesApp.API.Context;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CookRecipesApp.API.Endpoints
{
    public static class IngredientEndpoint
    {
        public static void MapIngredientEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/ingredients");

            //---------------------------------------------------------------Get previews
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


            //---------------------------------------------------------------Create ingredient
            group.MapPost("/create", async (IngredientCreateDto dto, ClaimsPrincipal user, CookRecipesDbContext db) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null) return Results.Unauthorized();

                if (await db.Ingredients.AnyAsync(i => i.Name.ToLower() == dto.Name.ToLower()))
                {
                    return Results.Conflict(new { Message = $"Ingredient with name '{dto.Name}' already exists." });
                }

                try
                {
                    var ingredient = new Ingredient {
                        Id = Guid.NewGuid(),
                        Name = dto.Name,
                        DefaultUnit = dto.DefaultUnitId,
                        Calories = dto.Calories,
                        Proteins = dto.Proteins,
                        Fats = dto.Fats,
                        Carbohydrates = dto.Carbohydrates,
                        Fiber = dto.Fiber,
                    };

                    foreach(var iu in dto.AdditionalUnits)
                    {
                        ingredient.IngredientUnits.Add(new IngredientUnit { IngredientId = ingredient.Id, UnitId = iu.UnitId, ToDefaultUnit = iu.ToDefaultUnit });
                    }

                    db.Ingredients.Add(ingredient);
                    await db.SaveChangesAsync();

                    return Results.Ok("Ingredient added");
                }
                catch (Exception ex)
                {
                    return Results.Problem("An error occurred while saving the ingredient.");
                }

            }).RequireAuthorization();



        }

    }
}
