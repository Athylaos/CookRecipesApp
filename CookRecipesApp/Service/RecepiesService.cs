using System;
using System.Collections.Generic;
using System.Text;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Model.Ingredient;
using CookRecipesApp.Model.Category;
using SQLite;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics.Text;

namespace CookRecipesApp.Service
{
    public interface IRecepiesService
    {
        public Task<List<Recepie>> GetAllRecepiesAsync();
        public Task<Recepie> GetRecepieAsync(int id);

        public Task SaveRecepieAsync(Recepie recepie);
        public Task DeleteRecepieAsync(int id);
        public Task UpdateRecepieAsync(Recepie recepie);

    }

    public class RecepiesService : IRecepiesService
    {
        private readonly ISQLiteAsyncConnection _database;
        private readonly IngredientsService _ingredientService;
        private readonly CategoryService _categoryService;

        public RecepiesService(SQLiteConnectionFactory factory, IngredientsService ingredientsService, CategoryService categoryService)
        {
            _database = factory.CreateConnection();
            _ingredientService = ingredientsService;
            _categoryService = categoryService;
        }

        private async Task<RecepieIngredient> DbToRecepieIngredientAsync(RecepieIngredientDbModel dbModel)
        {
            var ingredient = await _ingredientService.GetIngredientAsync(dbModel.IngredientId);
            var unit = await _database.Table<UnitDbModel>().FirstOrDefaultAsync(u => u.Id == dbModel.UnitId);

            return new RecepieIngredient
            {
                Id = dbModel.Id,
                RecepieId = dbModel.RecepieId,
                Ingredient = ingredient,
                Quantity = dbModel.Quantity,
                SelectedUnit = unit,
            };
        }

        private RecepieIngredientDbModel RecepieIngredientToDbAsync(RecepieIngredient recepie)
        {
            return new RecepieIngredientDbModel
            {
                Id = recepie.Id,
                RecepieId = recepie.RecepieId,
                IngredientId = recepie.Ingredient.Id,
                Quantity = recepie.Quantity,
                UnitId = recepie.SelectedUnit.Id,                            
            };
        }




        public async Task<List<RecepieIngredient>> GetAllIngredientsForRecepieAsync(int recepieId)
        {

            var dbModels = await _database.Table<RecepieIngredientDbModel>().Where(ri => ri.RecepieId == recepieId).ToListAsync();

            var result = new List<RecepieIngredient>();
            foreach (var model in dbModels)
            {
                result.Add(await DbToRecepieIngredientAsync(model));
            }
            return result;
        }      


        public async Task<Recepie> RecepieDbModelToRecepieAsync(RecepieDbModel recepieDbModel)
        {
            Recepie recepie = new Recepie()
            {
                Id = recepieDbModel.Id,
                UserId = recepieDbModel.UserId,
                Title = recepieDbModel.Title,
                //Add cookingStep
                CoockingTime = recepieDbModel.CoockingTime,
                Servings = recepieDbModel.Servings,
                ServingUnit = await _database.Table<UnitDbModel>().FirstOrDefaultAsync(x => x.Id == recepieDbModel.ServingUnitId),
                DifficultyLevel = (DifficultyLevel)recepieDbModel.Difficulty,
                Ingredients = await GetAllIngredientsForRecepieAsync(recepieDbModel.Id),
                Categories = await _categoryService.GetRecepieCategoriesAsync(recepieDbModel.Id),

                Calories = recepieDbModel.Calories,
                Proteins = recepieDbModel.Proteins,
                Fats = recepieDbModel.Fats,
                Carbohydrates = recepieDbModel.Carbohydrates,
                Fiber = recepieDbModel.Fiber,

                RecepieCreated = DateTime.Parse(recepieDbModel.RecepieCreated),
                Rating = recepieDbModel.Rating,
                UsersRated = recepieDbModel.UsersRated

            };
            return recepie;
        }

        public RecepieDbModel RecepieToRecepieDbModel(Recepie recepie)
        {
            RecepieDbModel recepieDbModel = new()
            {
                Id = recepie.Id,
                UserId = recepie.UserId,
                Title = recepie.Title,
                //Add cookingStep
                CoockingTime = recepie.CoockingTime,
                Servings = recepie.Servings,
                ServingUnitId = recepie.ServingUnit.Id,
                Difficulty = (int)recepie.DifficultyLevel,

                Calories = recepie.Calories,
                Proteins = recepie.Proteins,
                Fats = recepie.Fats,
                Carbohydrates = recepie.Carbohydrates,
                Fiber = recepie.Fiber,

                RecepieCreated = recepie.RecepieCreated.ToString(),
                Rating = recepie.Rating,
                UsersRated = recepie.UsersRated
            };            

            return recepieDbModel;
        }



        public async Task DeleteRecepieAsync(int id)
        {
            await _database.DeleteAsync<RecepieDbModel>(id);
            await _database.Table<RecepieIngredientDbModel>().DeleteAsync(x => x.RecepieId == id);
            await _database.Table<RecepieCategoryDbModel>().DeleteAsync(x => x.RecepieId == id);
        }

        public async Task<List<Recepie>> GetAllRecepiesAsync()
        {
            var recepieDbList = await _database.Table<RecepieDbModel>().ToListAsync();
            List<Recepie> recepieList = new();

            foreach (var recepie in recepieDbList)
            {
                recepieList.Add(await RecepieDbModelToRecepieAsync(recepie));
            }
            return recepieList;
            
        }

        public async Task<Recepie> GetRecepieAsync(int id)
        {
            var recepieDbModel = await _database.Table<RecepieDbModel>()
                                                .FirstOrDefaultAsync(r => r.Id == id);
            if (recepieDbModel == null) throw new ArgumentNullException("Object not found in database");
            return await RecepieDbModelToRecepieAsync(recepieDbModel);
        }

        public async Task SaveRecepieAsync(Recepie recepie)
        {
            if (recepie == null) throw new ArgumentNullException("Cant save null object to database");

            var recepieDbModel = RecepieToRecepieDbModel(recepie);
            await _database.InsertAsync(recepieDbModel);

            recepie.Id = recepieDbModel.Id;

            await SaveRecepieIngredientsAsync(recepie.Id, recepie.Ingredients);
            await SaveRecepieCategoriesAsync(recepie.Id, recepie.Categories);

            return;
        }

        public async Task UpdateRecepieAsync(Recepie recepie)
        {
            var recepieDbModel = RecepieToRecepieDbModel(recepie);

            await _database.UpdateAsync(recepieDbModel);
            await SaveRecepieIngredientsAsync(recepie.Id, recepie.Ingredients);
            await SaveRecepieCategoriesAsync(recepie.Id, recepie.Categories);

            return;
        }

        public async Task SaveRecepieIngredientsAsync(int recepieId, List<RecepieIngredient> ingredients)
        {
            await _database.Table<RecepieIngredientDbModel>().DeleteAsync(x => x.RecepieId == recepieId);

            var dbList = ingredients.Select(i => new RecepieIngredientDbModel
            {
                RecepieId = recepieId,
                IngredientId = i.Ingredient.Id,
                Quantity = i.Quantity,
                UnitId = i.SelectedUnit.Id
            }).ToList();

            await _database.InsertAllAsync(dbList);
        }

        public async Task SaveRecepieCategoriesAsync(int recepieId, List<Category> categories)
        {
            await _database.Table<RecepieCategoryDbModel>().DeleteAsync(rc => rc.RecepieId == recepieId);

            var links = categories.Select(c => new RecepieCategoryDbModel
            {
                RecepieId = recepieId,
                CategoryId = c.Id
            }).ToList();

            await _database.InsertAllAsync(links);
        }





    }
}
