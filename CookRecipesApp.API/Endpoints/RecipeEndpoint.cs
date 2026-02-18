using CookRecipesApp.API.Context;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CookRecipesApp.API.Endpoints
{
    public static class RecipeEndpoint
    {

        public static void MapRecipeEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/recipes");

            //Get recipes
            group.MapGet("/get", async (int? amount, CookRecipesDbContext db) =>
            {
                var query = db.Recipes.Include(r => r.User).OrderByDescending(r => r.RecipeCreated);

                if (amount != 0 && amount != null)
                {
                    return await query.Take(amount.Value).ToListAsync();
                }

                return await query.ToListAsync();
            });

            group.MapGet("/getPreviews", async (int? amount, CookRecipesDbContext db) =>
            {
                var query = db.Recipes
                    .OrderByDescending(r => r.RecipeCreated)
                    .Select(r => new RecipePreviewDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        PhotoUrl = r.PhotoUrl,
                        CookingTime = r.CookingTime,
                        ServingsAmount = r.ServingsAmount,
                        Difficulty = r.Difficulty,
                        Rating = r.Rating,
                        UserName = r.User.Name,
                        Calories = r.Calories,
                    });

                if (amount != 0 && amount != null)
                {
                    return await query.Take(amount.Value).ToListAsync();
                }
                else
                {
                    return await query.ToListAsync();
                }
            });






        }


    }
}
