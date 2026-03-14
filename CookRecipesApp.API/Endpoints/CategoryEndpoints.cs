using CookRecipesApp.API.Context;
using CookRecipesApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CookRecipesApp.API.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/categories");

            //---------------------------------------------------------------Get all categories
            group.MapGet("/getAll", async (CookRecipesDbContext db) =>
            {
                return await db.Categories.AsNoTracking().OrderBy(c => c.SortOrder).ToListAsync();
            });


            //---------------------------------------------------------------Get main categories
            group.MapGet("/getMain", async (CookRecipesDbContext db) =>
            {
                return await db.Categories.AsNoTracking().Where(c => c.ParentCategory == null).OrderBy(c => c.SortOrder).ToListAsync();                   
            });

            group.MapGet("/get/{categoryId:guid}", async (Guid categoryId, ClaimsPrincipal user, CookRecipesDbContext db) =>
            {
                var category = await db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == categoryId);

                if(category is null)
                {
                    return Results.BadRequest(category);
                }
                else
                {
                    return Results.Ok(category);
                }


            });




        }
    }
}
