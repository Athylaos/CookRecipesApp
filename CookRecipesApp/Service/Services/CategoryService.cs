using CookRecipesApp.Service.Interface;
using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Service.Services
{
    internal class CategoryService : ICategoryService
    {
        public Task<List<Category>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> GetChildCategoriesAsync(Guid parentId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> GetMainCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Category>> GetRecepieCategoriesAsync(Guid recepieId)
        {
            throw new NotImplementedException();
        }
    }
}
