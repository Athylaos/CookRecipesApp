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

            //---------------------------------------------------------------Get recipes
            group.MapGet("/get", async (int? amount, CookRecipesDbContext db) =>
            {
                var query = db.Recipes.Include(r => r.User).OrderByDescending(r => r.RecipeCreated);

                if (amount != 0 && amount != null)
                {
                    return await query.Take(amount.Value).ToListAsync();
                }

                return await query.ToListAsync();
            });

            //---------------------------------------------------------------Get previews
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

            //---------------------------------------------------------------Get previews filtered
            group.MapGet("/getPreviews/filtered", async ([AsParameters] RecipeFilterParametrs filter, ClaimsPrincipal user, CookRecipesDbContext db) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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

            //---------------------------------------------------------------Toggle favorite recipe
            group.MapPost("/toggleFavorite/{recipeId:guid}", async (Guid recipeId, ClaimsPrincipal user, CookRecipesDbContext db) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var favorite = await db.RecipesUsers.FirstOrDefaultAsync(ru => ru.RecipesId == recipeId && ru.UsersId == userId);

                if (favorite == null)
                {
                    db.RecipesUsers.Add(new RecipesUser()
                    {
                        RecipesId = recipeId,
                        UsersId = userId,
                        IsFavorite = true
                    });
                    await db.SaveChangesAsync();
                    return Results.Ok(new { isFavorite = true });
                }
                else
                {
                    favorite.IsFavorite = false;
                    db.RecipesUsers.Update(favorite);
                    await db.SaveChangesAsync();
                    return Results.Ok(new { isFavorite = false });
                }
            }).RequireAuthorization();


            //---------------------------------------------------------------Get recipe details
            group.MapGet("/getRecipeDetails/{recipeId:guid}", async (Guid recipeId, ClaimsPrincipal user, CookRecipesDbContext db) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Guid? currentUserId = userIdClaim != null ? Guid.Parse(userIdClaim) : null;

                var recipe = await db.Recipes.AsNoTracking().Where(r => r.Id == recipeId).Select(r => new RecipeDetailsDto()
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    Title = r.Title,
                    PhotoUrl = r.PhotoUrl,
                    CookingTime = r.CookingTime,
                    ServingsAmount = r.ServingsAmount,
                    Difficulty = (DifficultyLevel)r.Difficulty,
                    Calories = r.Calories,
                    Proteins = r.Proteins,
                    Fats = r.Fats,
                    Carbohydrates = r.Carbohydrates,
                    Fiber = r.Fiber,
                    RecipeCreated = r.RecipeCreated,
                    Rating = r.Rating,
                    UsersRated = r.UsersRated,
                    Comments = r.Comments,
                    RecipeIngredients = r.RecipeIngredients,
                    RecipeSteps = r.RecipeSteps,
                    ServingUnit = r.ServingUnitNavigation,
                    UserName = r.User.Name,
                    UserSurname = r.User.Surname,
                    Categories = r.Categories,

                    IsFavorite = currentUserId != null && r.RecipesUsers.Any(ru => ru.UsersId == currentUserId && ru.IsFavorite)
                }).FirstOrDefaultAsync();

                return recipe;
            });


            //---------------------------------------------------------------Create recipe
            group.MapPost("/create", async (RecipeCreateDto dto, ClaimsPrincipal user, CookRecipesDbContext db) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null) return Results.Unauthorized();

                var userId = Guid.Parse(userIdClaim);

                var ingredientIds = dto.RecipeIngredients.Select(x => x.IngredientId).ToList();
                var dbIngredients = await db.Ingredients.Where(x => ingredientIds.Contains(x.Id)).ToListAsync();

                var dbCategories = await db.Categories.Where(x => dto.CategoriesIds.Contains(x.Id)).ToListAsync();

                var newRecipe = new Recipe()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Title = dto.Title,
                    PhotoUrl = dto.PhotoUrl,
                    CookingTime = dto.CookingTime,
                    ServingsAmount = dto.ServingsAmount,
                    ServingUnit = dto.ServingUnit,
                    Difficulty = dto.Difficulty,
                    RecipeCreated = DateTime.UtcNow,
                    Calories = 0,
                    Proteins = 0,
                    Fats = 0,
                    Carbohydrates = 0,
                    Fiber = 0
                };

                foreach (var i in dto.RecipeIngredients)
                {
                    var dbIng = dbIngredients.FirstOrDefault(x => x.Id == i.IngredientId);
                    if(dbIng != null)
                    {
                        newRecipe.Calories += (i.ConversionFactor * i.Quantity / dto.ServingsAmount) * dbIng.Calories;
                        newRecipe.Proteins += (i.ConversionFactor * i.Quantity / dto.ServingsAmount) * dbIng.Proteins;
                        newRecipe.Fats += (i.ConversionFactor * i.Quantity / dto.ServingsAmount) * dbIng.Fats;
                        newRecipe.Carbohydrates += (i.ConversionFactor * i.Quantity / dto.ServingsAmount) * dbIng.Carbohydrates;
                        newRecipe.Fiber += (i.ConversionFactor * i.Quantity / dto.ServingsAmount) * dbIng.Fiber;
                    }


                    newRecipe.RecipeIngredients.Add(new RecipeIngredient
                    {

                        RecipeId = newRecipe.Id,
                        IngredientId = i.IngredientId,
                        Quantity = i.Quantity,
                        UnitId = i.UnitId,
                        ConversionFactor = i.ConversionFactor
                    });
                }

                foreach (var step in dto.RecipeSteps)
                {
                    newRecipe.RecipeSteps.Add(new RecipeStep
                    {
                        Id = Guid.NewGuid(),
                        RecipeId = newRecipe.Id,
                        Description = step.Description,
                        StepNumber = step.StepNumber
                    });
                }

                foreach (var category in dbCategories)
                {
                    newRecipe.Categories.Add(category);
                }

                db.Recipes.Add(newRecipe);
                await db.SaveChangesAsync();

                return Results.Ok(newRecipe.Id);
            }).RequireAuthorization();




        }


    }
}
