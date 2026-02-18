using CookRecipesApp.API.Context;
using CookRecipesApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CookRecipesApp.API.Endpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/categories");

            //Get all categories
            group.MapGet("/getAll", async (CookRecipesDbContext db) =>
            {
                return await db.Categories.OrderBy(c => c.SortOrder).ToListAsync();
            });


            //Get main categories
            group.MapGet("/getMain", async (CookRecipesDbContext db) =>
            {
                return await db.Categories.Where(c => c.ParentCategory == null).OrderBy(c => c.SortOrder).ToListAsync();                   
            });




        }
    }
}
