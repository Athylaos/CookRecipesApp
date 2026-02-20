using CookRecipesApp.API.Context;
using CookRecipesApp.Shared.DTOs;
using CookRecipesApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

            //Get previews
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

            group.MapGet("/getPreviews/filtered", async ([AsParameters] RecipeFilterParametrs filter, ClaimsPrincipal user, CookRecipesDbContext db) =>
            {
                var userIdClaim = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                Guid? currentUserId = userIdClaim != null ? Guid.Parse(userIdClaim) : null;

                var query = db.Recipes.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                    query = query.Where(r => r.Title.ToLower().Contains(filter.SearchTerm.ToLower()));

                if (filter.CategoryId.HasValue)
                    query = query.Where(r => r.Categories.Any(c => c.Id == filter.CategoryId));

                if (filter.MaxCookingTime.HasValue)
                    query = query.Where(r => r.CookingTime <= filter.MaxCookingTime);

                if (filter.MaxDifficulty.HasValue)
                    query = query.Where(r => r.Difficulty <= filter.MaxDifficulty);

                if (filter.OnlyFavorites)
                {
                    if (currentUserId == null) return Results.Unauthorized();
                    query = query.Where(r => r.RecipesUsers.Any(ru => ru.UsersId == currentUserId && ru.IsFavorite));
                }

                var results = await query
                    .OrderByDescending(r => r.RecipeCreated)
                    .Take(filter.Amount)
                    .Select(r => new RecipePreviewDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        PhotoUrl = r.PhotoUrl,
                        CookingTime = r.CookingTime,
                        Difficulty = r.Difficulty,
                        Rating = r.Rating,
                        UserName = r.User.Name,
                        IsFavorite = currentUserId != null && r.RecipesUsers.Any(ru => ru.UsersId == currentUserId && ru.IsFavorite)
                    })
                    .ToListAsync();

                return Results.Ok(results);
            });






        }


    }
}
