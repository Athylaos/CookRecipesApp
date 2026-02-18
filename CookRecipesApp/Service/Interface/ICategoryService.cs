using CookRecipesApp.Shared.Models;

namespace CookRecipesApp.Service.Interface
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetRecepieCategoriesAsync(Guid recepieId);

        public Task<List<Category>> GetAllCategoriesAsync();

        public Task<Category?> GetCategoryByIdAsync(Guid id);

        public Task<List<Category>> GetMainCategoriesAsync();

        public Task<List<Category>> GetChildCategoriesAsync(Guid parentId);
    }
}
