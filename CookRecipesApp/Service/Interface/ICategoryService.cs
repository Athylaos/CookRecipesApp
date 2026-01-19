using CookRecipesApp.Model.Category;

namespace CookRecipesApp.Service.Interface
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetRecepieCategoriesAsync(int recepieId);

        public Task<List<Category>> GetAllCategoriesAsync(bool root);

        public Task<Category?> GetCategoryByIdAsync(int id);
    }
}
